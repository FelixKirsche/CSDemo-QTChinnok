//@GeneratedCode
namespace QTChinnok.AspMvc.Models.App
{
    using System;
    ///
    /// Generated by the generator
    ///
    public partial class TrackFilter : Models.View.IFilterModel
    {
        ///
        /// Generated by the generator
        ///
        static TrackFilter()
        {
            ClassConstructing();
            ClassConstructed();
        }
        static partial void ClassConstructing();
        static partial void ClassConstructed();
        
        ///
        /// Generated by the generator
        ///
        public TrackFilter()
        {
            Constructing();
            
            Constructed();
        }
        partial void Constructing();
        partial void Constructed();
        
        ///
        /// Generated by the generator
        ///
        public IdType? AlbumId
        { get; set; }
        
        ///
        /// Generated by the generator
        ///
        public IdType? GenreId
        { get; set; }
        
        ///
        /// Generated by the generator
        ///
        public IdType? MediaTypeId
        { get; set; }
        
        ///
        /// Generated by the generator
        ///
        public System.String? Name
        { get; set; }
        
        ///
        /// Generated by the generator
        ///
        public System.String? Composer
        { get; set; }
        ///
        /// Generated by the generator
        ///
        public bool HasEntityValue => AlbumId != null || GenreId != null || MediaTypeId != null || Name != null || Composer != null;
        private bool show = true;
        ///
        /// Generated by the generator
        ///
        public bool Show => show;
        ///
        /// Generated by the generator
        ///
        public string CreateEntityPredicate()
        {
            var result = new System.Text.StringBuilder();
            
            if (AlbumId != null)
            {
                if (result.Length > 0)
                {
                    result.Append(" || ");
                }
                result.Append($"(AlbumId != null && AlbumId == {AlbumId})");
            }
            if (GenreId != null)
            {
                if (result.Length > 0)
                {
                    result.Append(" || ");
                }
                result.Append($"(GenreId != null && GenreId == {GenreId})");
            }
            if (MediaTypeId != null)
            {
                if (result.Length > 0)
                {
                    result.Append(" || ");
                }
                result.Append($"(MediaTypeId != null && MediaTypeId == {MediaTypeId})");
            }
            if (Name != null)
            {
                if (result.Length > 0)
                {
                    result.Append(" || ");
                }
                result.Append($"(Name != null && Name.Contains(\"{Name}\"))");
            }
            if (Composer != null)
            {
                if (result.Length > 0)
                {
                    result.Append(" || ");
                }
                result.Append($"(Composer != null && Composer.Contains(\"{Composer}\"))");
            }
            return result.ToString();
        }
        ///
        /// Generated by the generator
        ///
        public override string ToString()
        {
            System.Text.StringBuilder sb = new();
            if (AlbumId != null)
{
sb.Append($"AlbumId: {AlbumId} ");
}
if (GenreId != null)
{
sb.Append($"GenreId: {GenreId} ");
}
if (MediaTypeId != null)
{
sb.Append($"MediaTypeId: {MediaTypeId} ");
}
if (string.IsNullOrEmpty(Name) == false)
{
sb.Append($"Name: {Name} ");
}
if (string.IsNullOrEmpty(Composer) == false)
{
sb.Append($"Composer: {Composer} ");
}
            return sb.ToString();
        }
    }
}
