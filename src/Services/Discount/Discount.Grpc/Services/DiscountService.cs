using Discount.Grpc.Data;
using Discount.Grpc.Models;
using Grpc.Core;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Discount.Grpc.Services;

public class DiscountService(DiscountContext couponDbContext) : DiscountProtoService.DiscountProtoServiceBase
{
    public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
    {
        var coupon = await couponDbContext
            .Coupons
            .FirstOrDefaultAsync(x => x.ProductName == request.ProductName);

        coupon ??= new Coupon { ProductName = "No Discount", Amount = 0, Description = "No Discount Available" };

        var couponModel = coupon.Adapt<CouponModel>(); // could be done manually as well and bypass Mapster

        return couponModel;
    }

    public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
    {
        if (request.Coupon is null)
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid Request"));
        }

        var coupon = request.Coupon.Adapt<Coupon>();

        await couponDbContext.Coupons.AddAsync(coupon);

        await couponDbContext.SaveChangesAsync();

        var couponModel = coupon.Adapt<CouponModel>();

        return couponModel;
    }

    public override async Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
    {
        if (request.Coupon is null)
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid Request"));
        }

        var coupon = request.Coupon.Adapt<Coupon>();

        couponDbContext.Coupons.Update(coupon);

        await couponDbContext.SaveChangesAsync();

        var couponModel = coupon.Adapt<CouponModel>();

        return couponModel;
    }

    public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
    {
        if (request is null)
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid Request"));
        }

        var coupon = await couponDbContext
            .Coupons
            .FirstOrDefaultAsync(x => x.ProductName == request.ProductName);

        if(coupon == null)
        {
            throw new RpcException(new Status(StatusCode.NotFound, "Coupon No Found"));
        }

        couponDbContext.Coupons.Remove(coupon);

        await couponDbContext.SaveChangesAsync();

        return new DeleteDiscountResponse { Success = true };
    }
}
