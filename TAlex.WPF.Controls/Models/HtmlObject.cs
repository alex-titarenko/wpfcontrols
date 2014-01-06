using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace TAlex.WPF.Models
{
    internal abstract class HtmlObject : INotifyPropertyChanged, IDataErrorInfo
    {
        #region Properties

        private bool _isValid;
        protected IDictionary<string, string> ValidationErorrs = new Dictionary<string, string>();

        #endregion

        public abstract string ToHtml();

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        #region IDataErrorInfo Members

        public virtual string Error
        {
            get
            {
                return IsValid ? "Model is invalid." : null;
            }
        }

        public bool IsValid
        {
            get
            {
                return _isValid;
            }

            set
            {
                _isValid = value;
                OnPropertyChanged("IsValid");
            }
        }

        public virtual string this[string columnName]
        {
            get
            {
                string error = ValidateProperty(columnName);
                
                if (ValidationErorrs.ContainsKey(columnName))
                {
                    if (String.IsNullOrEmpty(error)) ValidationErorrs.Remove(columnName);
                    else ValidationErorrs[columnName] = error;
                }
                else if (!String.IsNullOrEmpty(error))
                {
                    ValidationErorrs.Add(columnName, error);
                }
                IsValid = !ValidationErorrs.Any();
                return error;
            }
        }

        protected abstract string ValidateProperty(string propertyName);

        #endregion
    }

    internal class HyperlinkObject : HtmlObject
    {
        #region Fields

        private string _url;
        private string _text;

        #endregion

        #region Properties

        public string Url
        {
            get
            {
                return _url;
            }

            set
            {
                _url = value;
                OnPropertyChanged("Url");
            }
        }

        public string Text
        {
            get
            {
                return (!String.IsNullOrEmpty(_text)) ? _text : _url;
            }

            set
            {
                _text = value;
                OnPropertyChanged("Text");
            }
        }

        #endregion

        public override string ToHtml()
        {
            return String.Format("<a href=\"{0}\">{1}</a>", Url, Text);
        }

        #region Validation

        protected override string ValidateProperty(string propertyName)
        {
            string error = String.Empty;
            switch (propertyName)
            {
                case "Url":
                    error = ValidateUrl();
                    break;
            }

            return error;
        }

        private string ValidateUrl()
        {
            if (String.IsNullOrEmpty(Url)) return "Url is empty.";

            Uri uri;
            if (Uri.TryCreate(Url, UriKind.Absolute, out uri))
                return null;
            else
                return "Url format is invalid.";
        }

        #endregion
    }

    internal class TableObject : HtmlObject
    {
        #region Fields

        private int _rows;
        private int _columns;
        private int _width;
        private string _widthUnit;

        #endregion

        #region Properties

        public int Rows
        {
            get
            {
                return _rows;
            }

            set
            {
                _rows = value;
                OnPropertyChanged("Rows");
            }
        }

        public int Columns
        {
            get
            {
                return _columns;
            }

            set
            {
                _columns = value;
                OnPropertyChanged("Columns");
            }
        }

        public int Width
        {
            get
            {
                return _width;
            }

            set
            {
                _width = value;
                OnPropertyChanged("Width");
            }
        }

        public string WidthUnit
        {
            get
            {
                return _widthUnit;
            }

            set
            {
                _widthUnit = value;
                OnPropertyChanged("WidthUnit");
            }
        }

        #endregion


        #region Constructors

        public TableObject()
        {
            Rows = 2;
            Columns = 2;
            Width = 100;
            WidthUnit = "%";
        }

        #endregion

        public override string ToHtml()
        {
            int rows = Rows;
            int cols = Columns;
            string width = string.Format(" width=\"{0}{1}\"", Width, WidthUnit);

            StringBuilder bx = new StringBuilder();
            bx.AppendFormat("<table{0}>", width);
            for (int i = 0; i < rows; i++)
            {
                bx.Append("<tr>");

                for (int j = 0; j < cols; j++)
                {
                    bx.Append("<td></td>");
                }
                bx.Append("</tr>");
            }
            bx.Append("</table>");

            return bx.ToString();
        }

        protected override string ValidateProperty(string propertyName)
        {
            return null;
        }
    }
}
