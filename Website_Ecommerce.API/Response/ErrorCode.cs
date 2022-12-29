using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Website_Ecommerce.API.Response
{
    public class ErrorCode
    {
        public const string Success = "00000000";
        public const string BadRequest = "00000001";
        public const string ValidateError = "00000010";
        public const string NotFound = "00000011";
        public const string NotEmpty = "00000100";
        public const string ExistedDB = "00000101";
        public const string ExcuteDB = "00000111";
        public const string Forbidden = "00001000";
        public const string ExistUserOrEmail = "00001001";
        public const string InvalidPassword = "00001011";
        public const string PasswordNotMatch = "00001111";
        public const string e00010000 = "00010000";
        public const string e00010001 = "00010001";
        public const string e00010011 = "00010011";
        public const string e00010111 = "00010111";
        public const string e00011111 = "00011111";
        public const string e00100000 = "00100000";
        public const string e00100001 = "00100001";
        public const string e00100011 = "00100011";
        public const string e00100111 = "00100111";
        public const string e00101111 = "00101111";
        public const string e00111111 = "00111111";
        public const string e01000000 = "01000000";
        public const string e01000001 = "01000001";
        public const string e01000010 = "01000010";
        public const string e01000011 = "01000011";
        public const string e01000100 = "01000100";
        public const string e01000101 = "01000101";
        public const string e01000111 = "01000111";
    }
}