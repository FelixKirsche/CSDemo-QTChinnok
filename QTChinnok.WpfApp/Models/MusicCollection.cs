using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTChinnok.WpfApp.Models
{
    public class MusicCollection
    {
        public MusicCollection()
        {

        }
        public MusicCollection(Logic.Models.App.MusicCollection entity)
        {
            Id = entity.Id;
            Name = entity.Name;
            Albums = entity.Albums.Select(x => new Album(x)).ToList();

            foreach (var item in entity.Albums)
            {
                if (AlbumNames.Length > 0)
                {
                    AlbumNames += $", {item.Title}";
                }
                else
                {
                    AlbumNames = $"{item.Title}";
                }
            }
        }
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string AlbumNames { get; set; } = string.Empty;
        public List<Models.Album> Albums { get; set; } = new();
    }
}