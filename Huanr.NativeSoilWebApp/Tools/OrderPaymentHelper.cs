using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace  Huanr.NativeSoilWebApp.Tools
{
    /// <summary>
    /// 订单支付辅助类
    /// </summary>
    public static class OrderPaymentHelper
    {
        /// <summary>
        /// 订单支付前状态检查及更新
        /// </summary>
        /// <param name="zeroService"></param>
        /// <param name="orderID">订单号</param>
        /// <param name="orderM">订单详情</param>
        /// <param name="msg">处理结果消息</param>
        /// <returns></returns>
        public static bool BeforeOrderPayCheckAndChangeStatus(ZeroDbs.Interfaces.IDbService zeroService, Guid orderID, out Huanr.Models.NativeSoil.tOrder orderM, out string msg)
        {
            bool flag = false;
            var db = zeroService.DbGet<Huanr.Models.NativeSoil.tOrder>();
            var list = db.Select<Huanr.Models.NativeSoil.tOrder>("OrderID='"+orderID+"'");
            if (list == null || list.Count < 1)
            {
                orderM = null;
                msg = "订单不存在";
                return flag;
            }
            orderM = list[0];
            if (orderM != null)
            {
                if (!orderM.OrderDeleteStatus)
                {
                    if (orderM.OrderStatus == 0 || orderM.OrderStatus == 1 || orderM.OrderStatus == 3)
                    {
                        bool InStatusEnum = true;
                        switch (orderM.OrderStatus)
                        {
                            case 0:
                                orderM.OrderStatusRemark = "首次支付中";
                                break;
                            case 1:
                                orderM.OrderStatusRemark = "再次支付中（上次支付中断）";
                                break;
                            case 3:
                                orderM.OrderStatusRemark = "再次支付中（上次支付失败）";
                                break;
                            default:
                                InStatusEnum = false;
                                break;
                        }
                        if (InStatusEnum)
                        {
                            orderM.OrderStatus = 1;
                            if(db.Update(orderM) > 0)
                            {
                                msg = "订单为可支付状态，订单状态成功切换为“支付中”";
                                flag = true;
                            }
                            else
                            {
                                msg = "订单状态更新失败";
                            }
                        }
                        else
                        {
                            msg = "订单状态异常（枚举值为：" + orderM.OrderStatus + "）";
                        }
                    }
                    else
                    {
                        msg="订单已经支付成功了";
                    }
                }
                else
                {
                    msg="不可用的订单参数";
                }
            }
            else
            {
                msg="不存在的订单参数";
            }
            return flag;
        }

        /// <summary>
        /// 订单支付成功状态检查及更新
        /// </summary>
        /// <param name="zeroService"></param>
        /// <param name="orderID">订单号</param>
        /// <param name="orderPayPlatformSerialNumber">支付平台流水号</param>
        /// <param name="orderM">订单详情</param>
        /// <param name="msg">处理结果消息</param>
        public static void OrderPaySucces(ZeroDbs.Interfaces.IDbService zeroService, Guid orderID, string orderPayPlatformSerialNumber, out Huanr.Models.NativeSoil.tOrder orderM, out string msg)
        {
            var db = zeroService.DbGet<Huanr.Models.NativeSoil.tOrder>();
            var list = db.Select<Huanr.Models.NativeSoil.tOrder>("OrderID='" + orderID + "'");
            if (list == null || list.Count < 1)
            {
                orderM = null;
                msg = "订单不存在";
                return;
            }
            orderM = list[0];
            if (orderM != null)
            {
                orderM.OrderPayPlatformSerialNumber = orderPayPlatformSerialNumber;
                if (!orderM.OrderDeleteStatus)
                {
                    if (orderM.OrderStatus == 0 || orderM.OrderStatus == 1 || orderM.OrderStatus == 3)
                    {
                        bool InStatusEnum = true;
                        switch (orderM.OrderStatus)
                        {
                            case 0:
                                orderM.OrderStatusRemark = "首次支付即支付成功";
                                break;
                            case 1:
                                orderM.OrderStatusRemark = "支付中断后再次支付成功";
                                break;
                            case 3:
                                orderM.OrderStatusRemark = "支付失败后再次支付成功";
                                break;
                            default:
                                InStatusEnum = false;
                                break;
                        }
                        if (InStatusEnum)
                        {
                            orderM.OrderStatus = 2;//标识为支付成功
                            if(db.Update(orderM) > 0)
                            {
                                msg = "订单为可支付状态，订单状态成功切换为“支付中”";
                            }
                            else
                            {
                                msg = "订单状态更新失败";
                            }
                        }
                        else
                        {
                            msg = "订单状态异常（枚举值为：" + orderM.OrderStatus + "）";
                        }
                    }
                    else
                    {
                        msg = "订单对支付成功已经进行了处理";
                    }
                }
                else
                {
                    msg = "不可用的订单参数（订单置为删除状态）";
                }
            }
            else
            {
                msg = "不存在的订单参数";
            }
        }

        /// <summary>
        /// 订单支付失败状态检查及更新
        /// </summary>
        /// <param name="zeroService"></param>
        /// <param name="orderID">订单号</param>
        /// <param name="orderPayPlatformSerialNumber">支付平台流水号</param>
        /// <param name="orderM">订单详情</param>
        /// <param name="msg">处理结果消息</param>
        public static void OrderPayFail(ZeroDbs.Interfaces.IDbService zeroService, Guid orderID, string orderPayPlatformSerialNumber, out Huanr.Models.NativeSoil.tOrder orderM, out string msg)
        {
            var db = zeroService.DbGet<Huanr.Models.NativeSoil.tOrder>();
            var list = db.Select<Huanr.Models.NativeSoil.tOrder>("OrderID='" + orderID + "'");
            if (list == null || list.Count < 1)
            {
                orderM = null;
                msg = "订单不存在";
                return;
            }
            orderM = list[0];
            if (orderM != null)
            {
                orderM.OrderPayPlatformSerialNumber = orderPayPlatformSerialNumber;
                if (!orderM.OrderDeleteStatus)
                {
                    if (orderM.OrderStatus == 0 || orderM.OrderStatus == 1 || orderM.OrderStatus == 2)
                    {
                        bool InStatusEnum = true;
                        switch (orderM.OrderStatus)
                        {
                            case 0:
                                orderM.OrderStatusRemark = "首次支付失败";
                                break;
                            case 1:
                                orderM.OrderStatusRemark = "支付中断后再次支付失败";
                                break;
                            case 2:
                                orderM.OrderStatusRemark = "支付成功后再次支付失败";
                                break;
                            default:
                                InStatusEnum = false;
                                break;
                        }
                        if (InStatusEnum)
                        {
                            if (orderM.OrderStatus != 2)
                            {
                                orderM.OrderStatus = 3;//标识为支付失败
                                if (db.Update(orderM) > 0)
                                {
                                    msg = "订单为可支付状态，订单状态成功切换为“支付中”";
                                }
                                else
                                {
                                    msg = "订单状态更新失败";
                                }
                            }
                            else
                            {
                                msg = "不做处理（订单原来的状态为支付成功）";
                            }
                        }
                        else
                        {
                            msg = "订单状态异常（枚举值为：" + orderM.OrderStatus + "）";
                        }
                    }
                    else
                    {
                        msg = "订单对支付失败已经进行了处理";
                    }
                }
                else
                {
                    msg = "不可用的订单参数（订单置为删除状态）";
                }
            }
            else
            {
                msg = "不存在的订单参数";
            }
        }

    }
}
