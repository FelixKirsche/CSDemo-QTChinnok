using QTChinnok.Logic.Controllers.App;
using QTChinnok.WpfApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace QTChinnok.WpfApp.ViewModels
{
    public class MusicCollectionViewModel : BaseViewModel
    {
        #region Fields
        private ICommand? _cmdSave;
        private ICommand? _cmdClose;
        private ICommand? _cmdDeleteAlbum;
        private ICommand? _cmdAddAlbum;
        private MusicCollection? _model;
        private List<Album> _albumList = new();
        public int? _albumId;
        #endregion
        #region Commands Props
        public ICommand CommandSave => RelayCommand.CreateCommand(ref _cmdSave, p => Save());
        public ICommand CommandClose => RelayCommand.CreateCommand(ref _cmdClose, p => Close());
        public ICommand CommandDeleteAlbum => RelayCommand.CreateCommand(ref _cmdDeleteAlbum, p => DeleteAlbum(), p => SelectedAlbum != null);
        public ICommand CommandAddAlbum => RelayCommand.CreateCommand(ref _cmdAddAlbum, p => AddAlbum());
        #endregion
        #region Properties
        public MusicCollection Model
        {
            get { return _model ??= new MusicCollection(); }
            set
            {
                _model = value;
                OnPropertyChanged(nameof(Name));
                OnPropertyChanged(nameof(CollectionAlbums));
            }
        }

        public ObservableCollection<Album> CollectionAlbums
        {
            get => new ObservableCollection<Album>(_model!.Albums);
            set => _model!.Albums = value.ToList();
        }
        public string Name
        {
            get => Model.Name;
            set { Model.Name = value; }
        }

        public int? AlbumId
        {
            get => _albumId;
            set => _albumId = value;
        }

        public Album[] AlbumList => _albumList.ToArray();
        public Album? SelectedAlbum { get; set; }
        #endregion

        public MusicCollectionViewModel()
        {
            OnPropertyChanged(nameof(AlbumList));
        }

        protected override void OnPropertyChanged(string propertyName)
        {
            if (propertyName == nameof(AlbumList))
            {
                Task.Run(LoadAlbumsAsync);
            }
            else
            {
                base.OnPropertyChanged(propertyName);
            }
        }
        #region Methods
        private async Task LoadAlbumsAsync()
        {
            using var ctrl = new AlbumsController();
            var items = await ctrl.GetAllAsync().ConfigureAwait(false);

            _albumList.Clear();
            _albumList.AddRange(items.OrderBy(i => i.Title).Select(x => new Album(x)));

            if ((AlbumId == 0 || AlbumId is null) && _albumList.Any())
            {
                AlbumId = _albumList.First().Id;
            }

            base.OnPropertyChanged(nameof(AlbumId));
            base.OnPropertyChanged(nameof(AlbumList));
        }

        private void Save()
        {
            bool error = false;
            string errorMessage = string.Empty;

            Task.Run(async () =>
            {
                using var ctrl = new Logic.Controllers.App.MusicCollectionsController();

                try
                {
                    var entity = ctrl.Create();

                    entity.CopyFrom(Model);

                    foreach (var item in Model.Albums)
                    {
                        var temp = new QTChinnok.Logic.Models.App.Album();
                        temp.CopyFrom(item);
                        entity.Albums.Add(temp);
                    }

                    if (Model.Id == 0)
                    {
                        await ctrl.InsertAsync(entity).ConfigureAwait(false);
                    }
                    else
                    {
                        await ctrl.UpdateAsync(entity).ConfigureAwait(false);
                    }
                    await ctrl.SaveChangesAsync().ConfigureAwait(false);
                }
                catch (System.Exception ex)
                {
                    error = true;
                    errorMessage = ex.Message;
                }

            }).Wait();

            if (error)
            {
                MessageBox.Show(errorMessage, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                Window?.Close();
            }
        }

        private void Close()
        {
            Window?.Close();
        }

        private void DeleteAlbum()
        {
            var selectedAlbum = Model.Albums.First(x => x.Id == SelectedAlbum!.Id);
            _model!.Albums.Remove(selectedAlbum);
            _albumList.Add(selectedAlbum);
            _albumList = _albumList.OrderBy(x => x.Title).ToList();
            AlbumId = SelectedAlbum?.Id;
            base.OnPropertyChanged(nameof(AlbumList));
            base.OnPropertyChanged(nameof(CollectionAlbums));
            base.OnPropertyChanged(nameof(AlbumId));
        }

        private void AddAlbum()
        {
            var selectedAlbum = AlbumList.First(x => x.Id == AlbumId);
            _model!.Albums.Add(selectedAlbum);
            _albumList.Remove(selectedAlbum);
            AlbumId = _albumList.FirstOrDefault()?.Id;
            base.OnPropertyChanged(nameof(AlbumList));
            base.OnPropertyChanged(nameof(CollectionAlbums));
        }
        #endregion
    }
}