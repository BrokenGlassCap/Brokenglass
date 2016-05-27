using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrokenGlassDomain.Entities
{
    public class UserRegistrationModel
    {
        //[RegularExpression(@"^\w+([+-.']\w+)*@sberbank\.ru",ErrorMessage ="Введен неверный Email")]
        public string Email { get; set; }
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[a-zA-Z\d\S]{8,}$",ErrorMessage ="Неверный формат пароля.")]
        [DataType(DataType.Password)]
        [StringLength(20,MinimumLength = 8, ErrorMessage ="Пароль должен быть от 8 до 20 символов.")]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Compare("Password",ErrorMessage ="Пароль и его подтверждение должны быть одинаковы.")]
        public string ConfirmPassword { get; set; }
    }
}
