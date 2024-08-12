using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imtiaz_AlArbani
{
    class RequestParameter
    {
        public class LoginClass
        {
            public string loginName { get; set; }
            public string Password { get; set; }
            public string userName { get; set; }
            public string roleID { get; set; }
            public string roleName { get; set; }
            public string status { get; set; }
            public string message { get; set; }
            public string validTill { get; set; }
            public string token { get; set; }
            public string rolrefreshTokeneName { get; set; }
            public string refreshTokenNew { get; set; }
            public string userID { get; set; }
        }
        public class getallsetting
        {
            public int get { get; set; }
        }
        public class getalldeviceClass
        {
            public int get { get; set; }
        }
        public class ActivatedeviceClass
        {
            public string message { get; set; }
            public string status { get; set; }
            public string deviceSerialNo { get; set; }
            public string deviceLicKey { get; set; }

            public int deviceID { get; set; }

        }
        public class DeletedeviceClass
        {
            public string message { get; set; }
            public int delete { get; set; }
            public int update { get; set; }
            public string deviceID { get; set; }
            public int LastUpdatedBy { get; set; }
        }
        public class SaveedeviceClass
        {
            public string message { get; set; }
            public int update { get; set; }
            public int deviceID { get; set; }
            public int LastUpdatedBy { get; set; }
            public string DeviceDesc { get; set; }

        }
        public class createsettingClass
        {
            public int add { get; set; }
            public int update { get; set; }
            public int id { get; set; }
            public int printingMode { get; set; }
            public int typeofLabel { get; set; }
            public string message { get; set; }
            public int createdBy { get; set; }
            public int lastUpdatedBy { get; set; }
        }
        public class getallbrandClass
        {
            public int get { get; set; }
        }
        public class getuncheckClass
        {
        }

        public class createbrandClass
        {
            public int add { get; set; }
            public int update { get; set; }
            public string brandName { get; set; }
            public string message { get; set; }
            public List<storess> storeBrandList { get; set; } = new List<storess> { };
            public int createdBy { get; set; }
            public string brandCode { get; set; }
            public int brandID { get; set; }
            public int lastUpdatedBy { get; set; }
        }

        public class brand
        {
            public string brandCode;
        }

        public class storess
        {
            public string StoreCode { get; set; }
        }

        public class generatetokenforemail
        {
        
                public string loginName { get; set; }
            public string message { get; set; }

        }

        public class forgotpassword
        {
            public string loginName { get; set; }
            public string newPassword { get; set; }
            public string token { get; set; }


            public string message { get; set; }
        }

        public class getallstoresClass
        {
            public int get { get; set; }
        }
        public class createstoresClass
        {
            public int add { get; set; }
            public int update { get; set; }
            public string storeCode { get; set; }
            public string storeName { get; set; }
            public string message { get; set; }
            public int createdBy { get; set; }
            public int storeID { get; set; }
            public int lastUpdatedBy { get; set; }
        }
        public class deletestoresClass
        {

            public int update { get; set; }
            public int delete { get; set; }
            public string message { get; set; }
            public int storeID { get; set; }
            public int lastUpdatedBy { get; set; }
            public string status { get; set; }
        }
        public class deletebrandClass
        {

            public int update { get; set; }
            public int delete { get; set; }
            public string message { get; set; }
            public int brandID { get; set; }
            public int lastUpdatedBy { get; set; }
        }
        public class deleteuserClass
        {

            public int update { get; set; }
            public int delete { get; set; }
            public string message { get; set; }
            public int id { get; set; }
            public int isDeleted { get; set; }
        }
        public class createuserClass
        {
            public int add { get; set; }
            public int update { get; set; }
            public string loginName { get; set; }
            public string userName { get; set; }
            public string storeCode { get; set; }
            public string password { get; set; }
            public string emailAddress { get; set; }
            public string message { get; set; }

            public int roleID { get; set; }

            public int userAccess { get; set; }

            public int id { get; set; }
           
        }

        public class importFileList
        {
     
            //public string StoreCode { get; set; }

            public string ItemCode {get;set;}
            public string Reference {get;set;}
            public string ItemDesc {get;set;}
            public string Size {get;set;}
            public string Colour {get;set;}
        
            public string BarCode {get;set;}
            //public string StoreID { get; set; }
            public string RegularPriceCode { get; set; }
            public string SalesPriceCode { get; set; }
            //public string StoreName {get;set;}
            public string BrandID  {get;set;}
            public string BrandName { get; set; }
            public string RegularPrice {get;set;}
            public string SalePrice {get;set;}
            public string SalesStartDate{get;set;}
            public string SalesEndDate { get; set; }
    }
        public class importFileLists
        {
            public string StoreCode { get; set; }
            public string RegularPriceCode { get; set; }
            public string SalesPriceCode { get; set; }
          
        }

        public class importClass
        {
            public List<importFileList> importData { get; set; } = new List<importFileList> { };

            public string message { get; set; }
        }



        public class importStoresClass
        {
            public List<importFileLists> importStoresPrices { get; set; } = new List<importFileLists> { };

            public string message { get; set; }
        }









        public class saveCompanyClass
        {
            public string companyName { get; set; }
            public string companyAddress { get; set; }
            public string country { get; set; }
            public string city { get; set; }
            public string state { get; set; }
            public string email { get; set; }
            public string contactNo { get; set; }
            public string imageBase64 { get; set; }
            public string message { get; set; }
            public int add { get; set; }
            public int update { get; set; }
            public int id { get; set; }
            public string refreshTokenNew { get; set; }
            public string userID { get; set; }
        }
        public class replyClass
        {
            public string message { get; set; }
           
        }
    }
}
