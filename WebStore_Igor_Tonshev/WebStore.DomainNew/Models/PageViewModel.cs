﻿using System;

namespace WebStore.DomainNew.Models
{
    public class PageViewModel
    {
        /// <summary>
        /// Общее количество элементов
        /// </summary>
        public int TotalItems { get; set; }

        /// <summary>
        /// Количество элементов на странице
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Текущая страница
        /// </summary>
        public int PageNumber { get; set; }

        /// <summary>
        /// Общее количество страниц
        /// </summary>
        public int TotalPages
            => (int)Math.Ceiling((decimal)TotalItems / PageSize);
    }
}
