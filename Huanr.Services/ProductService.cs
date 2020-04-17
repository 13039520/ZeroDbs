using System;

namespace Huanr.Services
{
    public class ProductService
    {
        private readonly ZeroDbs.Interfaces.IDbService zeroService = null;
        public ProductService(ZeroDbs.Interfaces.IDbService zeroService)
        {
            this.zeroService = zeroService;
        }

        #region -- tSendAddress --
        public ZeroDbs.Interfaces.Common.PageData<Huanr.Models.NativeSoil.tSendAddress> GetSendAddressPage(long page, long size, string where, string orderby, int threshold)
        {
            return this.zeroService.DataOperator.Page<Huanr.Models.NativeSoil.tSendAddress>(page, size, where, orderby, threshold);
        }
        public ZeroDbs.Interfaces.Common.PageData<Huanr.Models.NativeSoil.tSendAddress> GetSendAddressPage(long page, long size, string where, string orderby, string[] fieldNames)
        {
            return this.zeroService.DataOperator.Page<Huanr.Models.NativeSoil.tSendAddress>(page, size, where, orderby, fieldNames);
        }
        public Huanr.Models.NativeSoil.tSendAddress GetSendAddress(Guid addrId)
        {
            return this.zeroService.DataOperator.Get<Huanr.Models.NativeSoil.tSendAddress>(addrId);
        }
        public int InsertSystemMenu(Huanr.Models.NativeSoil.tSendAddress addr)
        {
            return this.zeroService.DataOperator.Insert<Huanr.Models.NativeSoil.tSendAddress>(addr);
        }
        public int UpdateSystemMenu(Huanr.Models.NativeSoil.tSendAddress addr)
        {
            return this.zeroService.DataOperator.Update<Huanr.Models.NativeSoil.tSendAddress>(addr);
        }
        public int UpdateSystemMenu(System.Collections.Specialized.NameValueCollection nvc)
        {
            return this.zeroService.DataOperator.Update<Huanr.Models.NativeSoil.tSendAddress>(nvc);
        }
        public int DeleteSystemMenu(Huanr.Models.NativeSoil.tSendAddress addr)
        {
            var trans = this.zeroService.GetDbTransactionScope<Huanr.Models.NativeSoil.tProduct>(System.Data.IsolationLevel.ReadUncommitted);
            var reval = 0;
            trans.Execute(cmd => {
                cmd.CommandText = cmd.DbSqlBuilder.BuildSqlDelete<Huanr.Models.NativeSoil.tSendAddressTETFee>("FeeTemplateID IN(SELECT TemplateID FROM T_SendAddressTET WHERE TemplateSendAddressID='" + addr.AddrID + "')");
                reval += cmd.ExecuteNonQuery();
                cmd.CommandText = cmd.DbSqlBuilder.BuildSqlDelete<Huanr.Models.NativeSoil.tSendAddressTET>("TemplateSendAddressID='" + addr.AddrID + "'");
                reval += cmd.ExecuteNonQuery();
                cmd.CommandText = cmd.DbSqlBuilder.BuildSqlDelete<Huanr.Models.NativeSoil.tSendAddress>("AddrID='" + addr.AddrID + "'");
                reval += cmd.ExecuteNonQuery();

                trans.Complete(true);
            });
            return reval;
        }
        #endregion

        #region -- tSendAddressTET --
        public ZeroDbs.Interfaces.Common.PageData<Huanr.Models.NativeSoil.tSendAddressTET> GetSendAddressTETPage(long page, long size, string where, string orderby, int threshold)
        {
            return this.zeroService.DataOperator.Page<Huanr.Models.NativeSoil.tSendAddressTET>(page, size, where, orderby, threshold);
        }
        public ZeroDbs.Interfaces.Common.PageData<Huanr.Models.NativeSoil.tSendAddressTET> GetSendAddressTETPage(long page, long size, string where, string orderby, string[] fieldNames)
        {
            return this.zeroService.DataOperator.Page<Huanr.Models.NativeSoil.tSendAddressTET>(page, size, where, orderby, fieldNames);
        }
        public Huanr.Models.NativeSoil.tSendAddressTET GetSendAddressTET(Guid templateId)
        {
            return this.zeroService.DataOperator.Get<Huanr.Models.NativeSoil.tSendAddressTET>(templateId);
        }
        public int InsertSendAddressTET(Huanr.Models.NativeSoil.tSendAddressTET template)
        {
            return this.zeroService.DataOperator.Insert<Huanr.Models.NativeSoil.tSendAddressTET>(template);
        }
        public int UpdateSendAddressTET(Huanr.Models.NativeSoil.tSendAddressTET template)
        {
            return this.zeroService.DataOperator.Update<Huanr.Models.NativeSoil.tSendAddressTET>(template);
        }
        public int UpdateSendAddressTET(System.Collections.Specialized.NameValueCollection nvc)
        {
            return this.zeroService.DataOperator.Update<Huanr.Models.NativeSoil.tSendAddressTET>(nvc);
        }
        public int DeleteSendAddressTET(Huanr.Models.NativeSoil.tSendAddressTET template)
        {
            var trans = this.zeroService.GetDbTransactionScope<Huanr.Models.NativeSoil.tProduct>(System.Data.IsolationLevel.ReadUncommitted);
            var reval = 0;
            trans.Execute(cmd => {
                cmd.CommandText = cmd.DbSqlBuilder.BuildSqlDelete<Huanr.Models.NativeSoil.tSendAddressTETFee>("FeeTemplateID='"+ template.TemplateID + "'");
                reval += cmd.ExecuteNonQuery();
                cmd.CommandText = cmd.DbSqlBuilder.BuildSqlDelete<Huanr.Models.NativeSoil.tSendAddressTET>("TemplateID='" + template.TemplateID + "'");
                reval += cmd.ExecuteNonQuery();

                trans.Complete(true);
            });
            return reval;
        }
        #endregion

        #region -- tSendAddressTETFee --
        public ZeroDbs.Interfaces.Common.PageData<Huanr.Models.NativeSoil.tSendAddressTETFee> GetSendAddressTETFeePage(long page, long size, string where, string orderby, int threshold)
        {
            return this.zeroService.DataOperator.Page<Huanr.Models.NativeSoil.tSendAddressTETFee>(page, size, where, orderby, threshold);
        }
        public ZeroDbs.Interfaces.Common.PageData<Huanr.Models.NativeSoil.tSendAddressTETFee> GetSendAddressTETFeePage(long page, long size, string where, string orderby, string[] fieldNames)
        {
            return this.zeroService.DataOperator.Page<Huanr.Models.NativeSoil.tSendAddressTETFee>(page, size, where, orderby, fieldNames);
        }
        public Huanr.Models.NativeSoil.tSendAddressTETFee GetSendAddressTETFee(Guid feeId)
        {
            return this.zeroService.DataOperator.Get<Huanr.Models.NativeSoil.tSendAddressTETFee>(feeId);
        }
        public int InsertSendAddressTETFee(Huanr.Models.NativeSoil.tSendAddressTETFee fee)
        {
            return this.zeroService.DataOperator.Insert<Huanr.Models.NativeSoil.tSendAddressTETFee>(fee);
        }
        public int UpdateSendAddressTETFee(Huanr.Models.NativeSoil.tSendAddressTETFee fee)
        {
            return this.zeroService.DataOperator.Update<Huanr.Models.NativeSoil.tSendAddressTETFee>(fee);
        }
        public int UpdateSendAddressTETFee(System.Collections.Specialized.NameValueCollection nvc)
        {
            return this.zeroService.DataOperator.Update<Huanr.Models.NativeSoil.tSendAddressTETFee>(nvc);
        }
        public int DeleteSendAddressTETFee(Huanr.Models.NativeSoil.tSendAddressTETFee fee)
        {
            return this.zeroService.DataOperator.Delete<Huanr.Models.NativeSoil.tSendAddressTETFee>(fee);
        }
        #endregion

        #region -- tProduct --
        public ZeroDbs.Interfaces.Common.PageData<Huanr.Models.NativeSoil.tProduct> GetProductPage(long page, long size, string where, string orderby, int threshold)
        {
            return this.zeroService.DataOperator.Page<Huanr.Models.NativeSoil.tProduct>(page, size, where, orderby, threshold);
        }
        public ZeroDbs.Interfaces.Common.PageData<Huanr.Models.NativeSoil.tProduct> GetProductPage(long page, long size, string where, string orderby, string[] fieldNames)
        {
            return this.zeroService.DataOperator.Page<Huanr.Models.NativeSoil.tProduct>(page, size, where, orderby, fieldNames);
        }
        public ZeroDbs.Interfaces.Common.PageData<Huanr.Models.NativeSoil.tProduct> GetProductPage(long page, long size, string where, string orderby, int threshold, string uniqueFieldName)
        {
            return this.zeroService.DataOperator.Page<Huanr.Models.NativeSoil.tProduct>(page, size, where, orderby, threshold, uniqueFieldName);
        }
        public ZeroDbs.Interfaces.Common.PageData<Huanr.Models.NativeSoil.tProduct> GetProductPage(long page, long size, string where, string orderby, string[] fieldNames, string uniqueFieldName)
        {
            return this.zeroService.DataOperator.Page<Huanr.Models.NativeSoil.tProduct>(page, size, where, orderby, fieldNames, uniqueFieldName);
        }
        public Huanr.Models.NativeSoil.tProduct GetProduct(Guid productId)
        {
            return this.zeroService.DataOperator.Get<Huanr.Models.NativeSoil.tProduct>(productId);
        }
        public int InsertProduct(Huanr.Models.NativeSoil.tProduct product)
        {
            return this.zeroService.DataOperator.Insert<Huanr.Models.NativeSoil.tProduct>(product);
        }
        public int UpdateProduct(Huanr.Models.NativeSoil.tProduct product)
        {
            return this.zeroService.DataOperator.Update<Huanr.Models.NativeSoil.tProduct>(product);
        }
        public int UpdateProduct(System.Collections.Specialized.NameValueCollection nvc)
        {
            return this.zeroService.DataOperator.Update<Huanr.Models.NativeSoil.tProduct>(nvc);
        }
        public int DeleteProduct(Huanr.Models.NativeSoil.tProduct product)
        {
            return this.zeroService.DataOperator.Delete<Huanr.Models.NativeSoil.tProduct>(product);
        }
        #endregion

    }
}
