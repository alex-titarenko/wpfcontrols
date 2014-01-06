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
                return _text;
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

                case "Text":
                    error = ValidateText();
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

        private string ValidateText()
        {
            return String.IsNullOrEmpty(Text) ? "Text is empty" : null;
        }

        #endregion
    }
}
