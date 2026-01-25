using Microsoft.EntityFrameworkCore;
using Store.G02.Domain.Contracts;
using Store.G02.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.G02.Persistence
{
    public static class SpecificationsEvaluator
    {
        // _context.Products.Include(P => P.Brand).Include(P => P.Type).Where(P => P.Id == key as int?).FirstOrDefaultAsync() as TEntity;

        // Generate Dynamic Query
        public static IQueryable<TEntity> GetQuery<TKey, TEntity>(IQueryable<TEntity> inputQuery, ISpecifications<TKey, TEntity> spec)
            where TEntity : BaseEntity<TKey>
        {
            var query = inputQuery; // _context.Products

            // check if there is any criteria

            if (spec.Criteria is not null)
            {
                query = query.Where(spec.Criteria); // _context.Products.Where(P => P.Id == 12)
            }

            // check Expression for OrderBy or OrderByDescending
            if (spec.OrderBy is not null)
            {
                query = query.OrderBy(spec.OrderBy); // _context.Products.OrderBy(P => P.Name)
            }
            else if (spec.OrderByDescending is not null)
            {
                query = query.OrderByDescending(spec.OrderByDescending); // _context.Products.OrderByDescending(P => P.Name)
            }

            // _context.Products.Where(P => P.Id == 12).Include(P => P.Brand)
            // _context.Products.OrderBy(P => P.Name).Where(P => P.Id == 12).Include(P => P.Brand).Include(P => P.Type)
            query = spec.Includes.Aggregate(query, (query, includeExpression) => query.Include(includeExpression));

            return query;
        }
    }
}
