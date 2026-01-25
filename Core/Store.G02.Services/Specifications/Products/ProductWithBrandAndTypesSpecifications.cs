using Store.G02.Domain.Entities.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Store.G02.Services.Specifications.Products
{
    public class ProductsWithBrandAndTypeSpecifications : BaseSpecifications<int, Product>
    {
        public ProductsWithBrandAndTypeSpecifications(int id) : base(P => P.Id == id)
        {
            ApplyIncludes();
        }



        // null & null
        public ProductsWithBrandAndTypeSpecifications(int? brandId, int? typeId, string? sort, string? search, int? pageIndex, int? pageSize) : base
            (
            P =>
             (!brandId.HasValue || P.BrandId == brandId) 
            &&
             (!typeId.HasValue || P.TypeId == typeId)
            &&
             (string.IsNullOrEmpty(search) || P.Name.ToLower().Contains(search.ToLower()))
            )
        {
            ApplyPagination(pageSize.Value, pageIndex.Value);
            ApplySort(sort);
            ApplyIncludes();
        }
        


        private void ApplySort(string? sort)
        {
            // priceasc
            // pricedesc
            // nameasc
            if (!string.IsNullOrEmpty(sort))
            {
                switch (sort.ToLower())
                {
                    case "priceasc":
                        AddOrderBy(P => P.Price);
                        break;
                    case "pricedesc":
                        AddOrderByDescending(P => P.Price);
                        break;
                    case "nameasc":
                        AddOrderBy(P => P.Name);
                        break;
                    case "namedesc":
                        AddOrderByDescending(P => P.Name);
                        break;
                    default:
                        AddOrderBy(P => P.Name);
                        break;
                }
            }
            else
            {
                AddOrderBy(P => P.Name);
            }
        }
        private void ApplyIncludes()
        {
            Includes.Add(P => P.Brand);
            Includes.Add(P => P.Type);
        }
    }

}
