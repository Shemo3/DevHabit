﻿using Microsoft.EntityFrameworkCore;

namespace DevHabit.Api.DTOs.Common;

public sealed record PaginationResult<T> : ICollectionResponse<T>, ILinksResponse
{
    public int Page { get; init; }

    public int PageSize { get; init; }

    public int TotalCount { get; init; }

    public List<LinkDto> Links { get; set; }

    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);

    public bool HasPreviousPage => Page > 1;

    public bool HasNextPage => Page < TotalPages;

    public List<T> Items { get; init; }

    public static async Task<PaginationResult<T>> CreateAsync(IQueryable<T> source, int page, int pageSize)
    {
        int count = await source.CountAsync();
        List<T> items = await source
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        return new PaginationResult<T>
        {
            Page = page,
            PageSize = pageSize,
            TotalCount = count,
            Items = items
        };
    }
}
