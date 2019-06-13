using System;

namespace WTP.BLL.DTOs.ServicesDTOs
{
    public class PageDto
    {
        public int PageNumber { get; private set; } //Current page index
        public int TotalPages { get; private set; } //Total count of pages

        public PageDto(int count, int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
        }

        public bool HasPreviousPage
        {
            get
            {
                return (PageNumber > 1);
            }
        }

        public bool HasNextPage
        {
            get
            {
                return (PageNumber < TotalPages);
            }
        }

    }
}
