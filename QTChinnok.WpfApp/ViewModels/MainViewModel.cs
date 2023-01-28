//@CustumCode
//MdStart
using QTChinnok.Logic.Entities.App;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace QTChinnok.WpfApp.ViewModels
{
    using TGenre = Models.Genre;
    using TMediaType = Models.MediaType;
    using TAlbum = Models.Album;
    using TTrack = Models.Track;
    public partial class MainViewModel : BaseViewModel
    {
        #region fields
        private ICommand? _cmdAddMusicCollection;
        private ICommand? _cmdEditMusicCollection;
        private ICommand? _cmdDeleteMusicCollection;

        private ICommand? _cmdAddGenre;
        private ICommand? _cmdEditGenre;
        private ICommand? _cmdDeleteGenre;

        private ICommand? _cmdAddMediaType;
        private ICommand? _cmdEditMediaType;
        private ICommand? _cmdDeleteMediaType;

        private ICommand? _cmdAddAlbum;
        private ICommand? _cmdEditAlbum;
        private ICommand? _cmdDeleteAlbum;

        private ICommand? _cmdAddTrack;
        private ICommand? _cmdEditTrack;
        private ICommand? _cmdDeleteTrack;

        private string musicCollectionFilter = string.Empty;
        private string genreFilter = string.Empty;
        private string mediaTypeFilter = string.Empty;
        private string albumFilter = string.Empty;
        private string trackFilter = string.Empty;

        private List<TGenre> _genres = new();
        private List<TMediaType> _mediaTypes = new();
        private List<TAlbum> _albums = new();
        private List<TTrack> _tracks = new();
        private List<Models.MusicCollection> _musicCollections = new();
        #endregion fields

        #region properties
        public Models.MusicCollection[] MusicCollections => _musicCollections.ToArray();
        public TGenre[] Genres => _genres.ToArray();
        public TAlbum[] Albums => _albums.ToArray();
        public TTrack[] Tracks => _tracks.ToArray();
        public TMediaType[] MediaTypes => _mediaTypes.ToArray();

        public string MusicCollectionFilter
        {
            get => musicCollectionFilter;
            set
            {
                musicCollectionFilter = value;
                OnPropertyChanged(nameof(MusicCollections));
            }
        }
        public string GenreFilter
        {
            get => genreFilter;
            set
            {
                genreFilter = value;
                OnPropertyChanged(nameof(Genres));
            }
        }
        public string MediaTypeFilter
        {
            get => mediaTypeFilter;
            set
            {
                mediaTypeFilter = value;
                OnPropertyChanged(nameof(MediaTypes));
            }
        }
        public string AlbumFilter
        {
            get => albumFilter;
            set
            {
                albumFilter = value;
                OnPropertyChanged(nameof(Albums));
            }
        }
        public string TrackFilter
        {
            get => trackFilter;
            set
            {
                trackFilter = value;
                OnPropertyChanged(nameof(Tracks));
            }
        }
        public Models.MusicCollection? SelectedMusicCollection { get; set; }
        public TGenre? SelectedGenre { get; set; }
        public TMediaType? SelectedMediaType { get; set; }
        public TAlbum? SelectedAlbum { get; set; }
        public TTrack? SelectedTrack { get; set; }

        public ICommand CommandAddMusicCollection => RelayCommand.CreateCommand(ref _cmdAddMusicCollection, p => AddMusicCollection());
        public ICommand CommandEditMusicCollection => RelayCommand.CreateCommand(ref _cmdEditMusicCollection, p => EditMusicCollection(), p => SelectedMusicCollection != null);
        public ICommand CommandDeleteMusicCollection => RelayCommand.CreateCommand(ref _cmdDeleteMusicCollection, p => DeleteMusicCollection(), p => SelectedMusicCollection != null);

        public ICommand CommandAddGenre => RelayCommand.CreateCommand(ref _cmdAddGenre, p => AddGenre());
        public ICommand CommandEditGenre => RelayCommand.CreateCommand(ref _cmdEditGenre, p => EditGenre(), p => SelectedGenre != null);
        public ICommand CommandDeleteGenre => RelayCommand.CreateCommand(ref _cmdDeleteGenre, p => DeleteGenre(), p => SelectedGenre != null);

        public ICommand CommandAddMediaType => RelayCommand.CreateCommand(ref _cmdAddMediaType, p => AddMediaType());
        public ICommand CommandEditMediaType => RelayCommand.CreateCommand(ref _cmdEditMediaType, p => EditMediaType(), p => SelectedMediaType != null);
        public ICommand CommandDeleteMediaType => RelayCommand.CreateCommand(ref _cmdDeleteMediaType, p => DeleteMediaType(), p => SelectedMediaType != null);

        public ICommand CommandAddAlbum => RelayCommand.CreateCommand(ref _cmdAddAlbum, p => AddAlbum());
        public ICommand CommandEditAlbum => RelayCommand.CreateCommand(ref _cmdEditAlbum, p => EditAlbum(), p => SelectedAlbum != null);
        public ICommand CommandDeleteAlbum => RelayCommand.CreateCommand(ref _cmdDeleteAlbum, p => DeleteAlbum(), p => SelectedAlbum != null);

        public ICommand CommandAddTrack => RelayCommand.CreateCommand(ref _cmdAddTrack, p => AddTrack());
        #endregion properties

        public MainViewModel()
        {
            OnPropertyChanged(nameof(Genres));
            OnPropertyChanged(nameof(MediaTypes));
            OnPropertyChanged(nameof(Albums));
            OnPropertyChanged(nameof(Tracks));
            OnPropertyChanged(nameof(MusicCollections));
        }
        protected override void OnPropertyChanged(string propertyName)
        {
            if (propertyName == nameof(Genres))
            {
                Task.Run(LoadGenresAsync);
            }
            else if (propertyName == nameof(MediaTypes))
            {
                Task.Run(LoadMediaTypesAsync);
            }
            else if (propertyName == nameof(Albums))
            {
                Task.Run(LoadAlbumsAsync);
            }
            else if (propertyName == nameof(Tracks))
            {
                Task.Run(LoadTracksAsync);
            }
            else if (propertyName == nameof(MusicCollections))
            {
                Task.Run(LoadMusicCollectionsAsync);
            }
            else
            {
                base.OnPropertyChanged(propertyName);
            }
        }

        private void AddGenre()
        {
            GenreWindow window = new();

            window.ShowDialog();
            OnPropertyChanged(nameof(Genres));
        }
        private void EditGenre()
        {
            GenreWindow window = new();

            window.ViewModel.Model = SelectedGenre!;
            window.ShowDialog();
            OnPropertyChanged(nameof(Genres));
        }
        private void DeleteGenre()
        {
            var result = MessageBox.Show($"Soll der Eintrag '{SelectedGenre?.Name}' gelöscht werden", "Löschen", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                bool error = false;
                string errorMessage = string.Empty;

                Task.Run(async () =>
                {
                    try
                    {
                        using var ctrl = new Logic.Controllers.Base.GenresController();

                        await ctrl.DeleteAsync(SelectedGenre!.Id).ConfigureAwait(false);
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
                    MessageBox.Show(errorMessage, "Löschen", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    OnPropertyChanged(nameof(Genres));
                }
            }
        }

        private void AddMediaType()
        {
        }
        private void EditMediaType()
        {
        }
        private void DeleteMediaType()
        {
        }

        private void AddAlbum()
        {
            AlbumWindow window = new();

            window.ShowDialog();
            OnPropertyChanged(nameof(Albums));
        }
        private void EditAlbum()
        {
            AlbumWindow window = new();

            window.ViewModel.Model = SelectedAlbum!;
            window.ShowDialog();
            OnPropertyChanged(nameof(Albums));
        }
        private void DeleteAlbum()
        {
            var result = MessageBox.Show($"Soll der Eintrag '{SelectedAlbum?.Title}' gelöscht werden", "Löschen", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                bool error = false;
                string errorMessage = string.Empty;

                Task.Run(async () =>
                {
                    try
                    {
                        using var ctrl = new Logic.Controllers.App.AlbumsController();

                        await ctrl.DeleteAsync(SelectedAlbum!.Id).ConfigureAwait(false);
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
                    MessageBox.Show(errorMessage, "Löschen", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    OnPropertyChanged(nameof(Albums));
                }
            }
        }

        private void AddMusicCollection()
        {
            MusicCollectionWindow window = new();

            window.ShowDialog();
            OnPropertyChanged(nameof(MusicCollections));
        }

        private void EditMusicCollection()
        {
            throw new NotImplementedException();
        }
        private void DeleteMusicCollection()
        {
            throw new NotImplementedException();
        }

        private void AddTrack()
        {
            TrackWindow window = new();

            window.ShowDialog();
            OnPropertyChanged(nameof(Tracks));
        }

        private async Task LoadMusicCollectionsAsync()
        {
            using var ctrl = new Logic.Controllers.App.MusicCollectionsController();
            var items = await ctrl.GetAllAsync().ConfigureAwait(false);
            var result = items.Where(i => i.Name != null && i.Name.Contains(musicCollectionFilter, System.StringComparison.CurrentCultureIgnoreCase));

            _musicCollections.Clear();
            _musicCollections.AddRange(result.Select(i => new Models.MusicCollection(i)));
            SelectedMusicCollection = null;
            base.OnPropertyChanged(nameof(SelectedMusicCollection));
            base.OnPropertyChanged(nameof(MusicCollections));
        }

        private async Task LoadGenresAsync()
        {
            using var ctrl = new Logic.Controllers.Base.GenresController();
            var items = await ctrl.GetAllAsync().ConfigureAwait(false);
            var result = items.Where(i => i.Name != null && i.Name.Contains(genreFilter, System.StringComparison.CurrentCultureIgnoreCase));

            _genres.Clear();
            _genres.AddRange(result.Select(i => new TGenre(i)));
            SelectedGenre = null;
            base.OnPropertyChanged(nameof(SelectedGenre));
            base.OnPropertyChanged(nameof(Genres));
        }
        private async Task LoadMediaTypesAsync()
        {
            using var ctrl = new Logic.Controllers.Base.MediaTypesController();
            var items = await ctrl.GetAllAsync().ConfigureAwait(false);
            var result = items.Where(i => i.Name != null && i.Name.Contains(mediaTypeFilter, System.StringComparison.CurrentCultureIgnoreCase));

            _mediaTypes.Clear();
            _mediaTypes.AddRange(result.Select(i => new TMediaType(i)));
            SelectedMediaType = null;
            base.OnPropertyChanged(nameof(SelectedMediaType));
            base.OnPropertyChanged(nameof(MediaTypes));
        }
        private async Task LoadAlbumsAsync()
        {
            using var ctrl = new Logic.Controllers.App.AlbumsController();
            var items = await ctrl.GetAllAsync().ConfigureAwait(false);
            var result = items.Where(i => i.Title.Contains(albumFilter, System.StringComparison.CurrentCultureIgnoreCase)
                                       || i.Artist!.Name!.Contains(albumFilter, System.StringComparison.CurrentCultureIgnoreCase));

            _albums.Clear();
            _albums.AddRange(result.Select(i => new TAlbum(i)));
            SelectedAlbum = null;
            base.OnPropertyChanged(nameof(SelectedAlbum));
            base.OnPropertyChanged(nameof(Albums));
        }
        private async Task LoadTracksAsync()
        {
            using var ctrl = new Logic.Controllers.App.TracksController();
            var items = await ctrl.GetAllAsync().ConfigureAwait(false);
            var result = items.Where(i => i.Name.Contains(trackFilter, System.StringComparison.CurrentCultureIgnoreCase));

            _tracks.Clear();
            _tracks.AddRange(result.Select(i => new TTrack(i)));
            SelectedTrack = null;
            base.OnPropertyChanged(nameof(SelectedTrack));
            base.OnPropertyChanged(nameof(Tracks));
        }
    }
}
//MdEnd
