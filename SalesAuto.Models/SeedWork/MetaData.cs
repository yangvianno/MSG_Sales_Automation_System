namespace SalesAuto.Models.ViewModel.SeekWork
{
    public class MetaData
    {
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public bool HavePrevios => CurrentPage > 1;
        public bool HaveNext => CurrentPage < TotalPages;
    }
}