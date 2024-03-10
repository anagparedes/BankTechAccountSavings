﻿namespace BankTechAccountSavings.Domain.Entities
{
    public class Paginated<T>
    {
        public List<T>? Items { get; set; }
        public int TotalItems { get; set; }
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages => (int)Math.Ceiling(TotalItems / (double)PageSize);
    }
}
