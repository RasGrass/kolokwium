using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace WordSearcher
{
    /// <summary>
    /// Main class for View Model
    /// TODO: follow guidelines
    /// </summary>
    public class TextViewModel : ITextViewModel
    {
        private readonly IDispatcher _dispatcher;

        private string _content = Globals.LoremIpsum;

        private string _query;

        private MyCommand command;

        private List<string> _commands;

        private ASearcher _selectedMethod;

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        private string _searchResult;

        private bool _found = false;

        private List<ASearcher> _searchMethods;

        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }
        
        public TextViewModel(IDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
            _commands = new List<string>();
            _searchMethods = new List<ASearcher>() { new ContainsSearcher(), new StartsWithSearcher() };
        }


       

        public string Query
        {
            get
            {
                return _query;
            }
            set
            {
                NotifyPropertyChanged("Query");
                _query = value;
            }
        }

        public string Content
        {
            get
            {
                return _content;
            }
            set
            {
                NotifyPropertyChanged("Content");
                _content = value;
            }
        }

        public System.Windows.Input.ICommand SearchCommand
        {
            get {

                _dispatcher.RunOnUi(new Action(searchWrapper));
                return new MyCommand(new Action(searchWrapper)); 
            }
        }

        public string SearchResult
        {
            get
            {
                if (_found)
                {
                    return Globals.ResultsFound;
                }
                else
                {
                    return Globals.NoSearchResults;
                }
            }
            set
            {
                NotifyPropertyChanged("SearchResult");
                throw new NotImplementedException();
            }
        }

        public System.Windows.Input.ICommand SaveSearchesCommand
        {
            get { 
                if(PropertyChanged == null) {
                    PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs("SaveSearchesCommand"));
                }
                return null;
            }
        }

        public ASearcher SelectedMethod
        {
            get
            {
                if (_selectedMethod == null) { 
                    return SearchMethods.ElementAt(0);
                }
                else
                {
                    return _selectedMethod;
                }
            }
            set
            {
                NotifyPropertyChanged("SelectedMethod");
                _selectedMethod = value;
            }
        }

        public IEnumerable<ASearcher> SearchMethods
        {
            get {
                return _searchMethods;
            }
        }

        

        private bool search()
        {
            if(!(String.IsNullOrEmpty(Content) || String.IsNullOrEmpty(Query))){
                string[] words = _content.Split(new Char[]{' ','\n','\t'});
                foreach(string word in words) {
                if (SelectedMethod.VerifyText(word) == true ){
                    _searchResult = word;
                    return true;
                }
                }
            }
            return false;
        }

        private void searchWrapper()
        {
            _found = search();
        }
    }
}
