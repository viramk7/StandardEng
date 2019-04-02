using StandardEng.Common;
using StandardEng.Data.Repository;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StandardEng.Data.DB
{
    [MetadataType(typeof(Metadata))]
    public partial class tblRole : IValidatableObject
    {
        internal class Metadata
        {
            [ScaffoldColumn(false)]
            public int RoleId { get; set; }

            [Display(ResourceType = typeof(CommonMessage), Name = "Role", Order = 1)]
            [Required(ErrorMessageResourceName = "RoleNameRequired", ErrorMessageResourceType = typeof(CommonMessage))]
            [StringLength(50, ErrorMessageResourceName = "RoleNameLength", ErrorMessageResourceType = typeof(CommonMessage))]
            public string RoleName { get; set; }

            [Display(ResourceType = typeof(CommonMessage), Name = "Status", Order = 2)]
            public bool IsActive { get; set; }
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (CustomRepository.CheckRoleExistsOrNot(RoleName, RoleId))
            {
                var fieldName = new[] { "RoleName" };
                yield return new ValidationResult("Role is Already Exists.", fieldName);
            }
        }

    }

    [MetadataType(typeof(Metadata))]
    public partial class tblUser : IValidatableObject
    {
        internal class Metadata
        {
            [ScaffoldColumn(false)]
            public int UserId { get; set; }

            [Display(ResourceType = typeof(CommonMessage), Name = "Name", Order = 1)]
            [Required(ErrorMessageResourceName = "NameRequired", ErrorMessageResourceType = typeof(CommonMessage))]
            [StringLength(100, ErrorMessageResourceName = "NameLength", ErrorMessageResourceType = typeof(CommonMessage))]
            public string Name { get; set; }

            [Display(ResourceType = typeof(CommonMessage), Name = "Username", Order = 2)]
            [Required(ErrorMessageResourceName = "UsernameRequired", ErrorMessageResourceType = typeof(CommonMessage))]
            [StringLength(50, ErrorMessageResourceName = "UsernameLength", ErrorMessageResourceType = typeof(CommonMessage))]
            public string Username { get; set; }

            [DataType(DataType.Password)]
            [Display(ResourceType = typeof(CommonMessage), Name = "Password", Order = 3)]
            [Required(ErrorMessageResourceName = "PasswordRequired", ErrorMessageResourceType = typeof(CommonMessage))]
            [StringLength(15, ErrorMessageResourceName = "PasswordLength", ErrorMessageResourceType = typeof(CommonMessage))]
            public string Password { get; set; }

            [Display(Name = "User Contact Number", Order = 4)]
            [Required(ErrorMessageResourceName = "ContactNoRequired", ErrorMessageResourceType = typeof(CommonMessage))]
            [StringLength(10, ErrorMessageResourceName = "PhoneLength", ErrorMessageResourceType = typeof(CommonMessage))]
            [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid phone number")]
            public string UserContactNo { get; set; }

            [DataType(DataType.EmailAddress, ErrorMessageResourceName = "InvalidEmail", ErrorMessageResourceType = typeof(CommonMessage))]
            [Display(ResourceType = typeof(CommonMessage), Name = "Email", Order = 5)]
            //[Required(ErrorMessageResourceName = "EmailRequired", ErrorMessageResourceType = typeof(CommonMessage))]
            [UIHint("Email")]
            public string UserEmail { get; set; }

            [Display(Name = "User Address", Order = 6)]
            [Required(ErrorMessageResourceName = "UserAddressRequired", ErrorMessageResourceType = typeof(CommonMessage))]
            [UIHint("TextArea")]
            public string UserAddress { get; set; }

            [Display(ResourceType = typeof(CommonMessage), Name = "Role", Order = 7)]
            //[Required(ErrorMessageResourceName = "RoleNameRequired", ErrorMessageResourceType = typeof(CommonMessage))]
            [UIHint("GridForeignKey")]
            public Nullable<int> RoleId { get; set; }

            [Display(ResourceType = typeof(CommonMessage), Name = "UserCode", Order = 8)]
            [Required(ErrorMessageResourceName = "UserCodeRequired", ErrorMessageResourceType = typeof(CommonMessage))]
            [StringLength(3, ErrorMessageResourceName = "UserCodeLength", ErrorMessageResourceType = typeof(CommonMessage))]
            public string UserCode { get; set; }

            [Display(ResourceType = typeof(CommonMessage), Name = "Status", Order = 9)]
            public bool IsActive { get; set; }

            [ScaffoldColumn(false)]
            public bool IsSuperAdmin { get; set; }

            [ScaffoldColumn(false)]
            public string Token { get; set; }

            [ScaffoldColumn(false)]
            public DateTime? TokenExpiryDateTime { get; set; }

        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (CustomRepository.IsUserEmailExists(UserEmail, UserId))
            {
                var fieldName = new[] { "UserEmail" };
                yield return new ValidationResult("Email is already exists.", fieldName);
            }
            if (CustomRepository.IsUserNameExists(Username, UserId))
            {
                var fieldName = new[] { "Username" };
                yield return new ValidationResult("User Name is already exists.", fieldName);
            }
            if (CustomRepository.IsUserCodeExists(UserCode, UserId))
            {
                var fieldName = new[] { "UserCode" };
                yield return new ValidationResult("User Code is already exists.", fieldName);
            }
        }
    }

    [MetadataType(typeof(Metadata))]
    public partial class tblCountry : IValidatableObject
    {
        internal class Metadata
        {
            [ScaffoldColumn(false)]
            public int CountryId { get; set; }

            [Display(ResourceType = typeof(CommonMessage), Name = "CountryName", Order = 1)]
            [Required(ErrorMessageResourceName = "NameRequired", ErrorMessageResourceType = typeof(CommonMessage))]
            [StringLength(50, ErrorMessageResourceName = "CountryNameLength", ErrorMessageResourceType = typeof(CommonMessage))]
            public string CountryName { get; set; }

            [Display(ResourceType = typeof(CommonMessage), Name = "Status", Order = 2)]
            public bool IsActive { get; set; }
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (CustomRepository.IsCountryNameExists(CountryName, CountryId))
            {
                var fieldName = new[] { "CountryName" };
                yield return new ValidationResult("Country Name is Already Exists.", fieldName);
            }
        }

    }

    [MetadataType(typeof(Metadata))]
    public partial class tblState : IValidatableObject
    {
        internal class Metadata
        {
            [ScaffoldColumn(false)]
            public int StateId { get; set; }

            [Display(ResourceType = typeof(CommonMessage), Name = "StateName", Order = 1)]
            [Required(ErrorMessageResourceName = "NameRequired", ErrorMessageResourceType = typeof(CommonMessage))]
            [StringLength(50, ErrorMessageResourceName = "StateNameLength", ErrorMessageResourceType = typeof(CommonMessage))]
            public string StateName { get; set; }

            [Display(ResourceType = typeof(CommonMessage), Name = "Country", Order = 2)]
            [Required(ErrorMessageResourceName = "CountryRequired", ErrorMessageResourceType = typeof(CommonMessage))]
            [UIHint("GridForeignKey")]
            public int CountryId { get; set; }

            [Display(ResourceType = typeof(CommonMessage), Name = "Status", Order = 3)]
            public bool IsActive { get; set; }
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (CustomRepository.IsStateNameExists(StateName, StateId))
            {
                var fieldName = new[] { "StateName" };
                yield return new ValidationResult("State Name is Already Exists.", fieldName);
            }
        }

    }

    [MetadataType(typeof(Metadata))]
    public partial class tblCity : IValidatableObject
    {
        internal class Metadata
        {
            [ScaffoldColumn(false)]
            public int CityId { get; set; }

            [Display(ResourceType = typeof(CommonMessage), Name = "CityName")]
            [Required(ErrorMessageResourceName = "NameRequired", ErrorMessageResourceType = typeof(CommonMessage))]
            [StringLength(50, ErrorMessageResourceName = "CityNameLength", ErrorMessageResourceType = typeof(CommonMessage))]
            public string CityName { get; set; }

            [Display(ResourceType = typeof(CommonMessage), Name = "Country")]
            [Required(ErrorMessageResourceName = "CountryRequired", ErrorMessageResourceType = typeof(CommonMessage))]
            // [UIHint("GridForeignKey")]
            public int CountryId { get; set; }

            [Display(ResourceType = typeof(CommonMessage), Name = "State")]
            [Required(ErrorMessageResourceName = "StateRequired", ErrorMessageResourceType = typeof(CommonMessage))]
            // [UIHint("GridForeignKey")]
            public int StateId { get; set; }

            [Display(ResourceType = typeof(CommonMessage), Name = "Status")]
            public bool IsActive { get; set; }
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (CustomRepository.IsCityNameExists(CityName, CityId))
            {
                var fieldName = new[] { "CityName" };
                yield return new ValidationResult("City Name is Already Exists.", fieldName);
            }
        }

    }

    [MetadataType(typeof(Metadata))]
    public partial class tblCustomer : IValidatableObject
    {
        internal class Metadata
        {
            [ScaffoldColumn(false)]
            public int CustomerId { get; set; }

            [Display(ResourceType = typeof(CommonMessage), Name = "Name")]
            [Required(ErrorMessageResourceName = "NameRequired", ErrorMessageResourceType = typeof(CommonMessage))]
            [StringLength(50, ErrorMessageResourceName = "NameLength", ErrorMessageResourceType = typeof(CommonMessage))]
            public string CustomerName { get; set; }

            [Display(ResourceType = typeof(CommonMessage), Name = "Country")]
            [Required(ErrorMessageResourceName = "CountryRequired", ErrorMessageResourceType = typeof(CommonMessage))]
            public int CountryId { get; set; }

            [Display(ResourceType = typeof(CommonMessage), Name = "State")]
            [Required(ErrorMessageResourceName = "StateRequired", ErrorMessageResourceType = typeof(CommonMessage))]
            public int StateId { get; set; }

            [Display(ResourceType = typeof(CommonMessage), Name = "City")]
            [Required(ErrorMessageResourceName = "CityRequired", ErrorMessageResourceType = typeof(CommonMessage))]
            public int CityId { get; set; }

            [Display(ResourceType = typeof(CommonMessage), Name = "Addressline1")]
            public string Addressline1 { get; set; }

            [Display(ResourceType = typeof(CommonMessage), Name = "Addressline2")]
            public string Addressline2 { get; set; }

            [Display(ResourceType = typeof(CommonMessage), Name = "GSTNo")]
            [StringLength(15, ErrorMessageResourceName = "GSTLength", ErrorMessageResourceType = typeof(CommonMessage))]
            [MinLength(15, ErrorMessageResourceName = "GSTLength", ErrorMessageResourceType = typeof(CommonMessage))]
            public string GST { get; set; }

            [Display(ResourceType = typeof(CommonMessage), Name = "ContactNo")]
            [Required(ErrorMessageResourceName = "ContactNoRequired", ErrorMessageResourceType = typeof(CommonMessage))]
            [StringLength(10, ErrorMessageResourceName = "PhoneLength", ErrorMessageResourceType = typeof(CommonMessage))]
            [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid phone number")]
            public string ContactNo { get; set; }

            [Display(ResourceType = typeof(CommonMessage), Name = "AlternativeContactNo")]
            [StringLength(10, ErrorMessageResourceName = "PhoneLength", ErrorMessageResourceType = typeof(CommonMessage))]
            [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid phone number")]
            public string AlternativeContactNo { get; set; }

            [DataType(DataType.EmailAddress, ErrorMessageResourceName = "InvalidEmail", ErrorMessageResourceType = typeof(CommonMessage))]
            [Display(ResourceType = typeof(CommonMessage), Name = "Email")]
            public string Email { get; set; }

            [Display(ResourceType = typeof(CommonMessage), Name = "Status")]
            public bool IsActive { get; set; }

            [Display(ResourceType = typeof(CommonMessage), Name = "Region")]
            [Required(ErrorMessageResourceName = "RegionNameRequired", ErrorMessageResourceType = typeof(CommonMessage))]
            public Nullable<int> RegionId { get; set; }

            [Display(ResourceType = typeof(CommonMessage), Name = "PinCode")]
            [StringLength(6, ErrorMessageResourceName = "PinCodeLength", ErrorMessageResourceType = typeof(CommonMessage))]
            [MinLength(6, ErrorMessageResourceName = "PinCodeLength", ErrorMessageResourceType = typeof(CommonMessage))]
            public string PinCode { get; set; }

            [Display(ResourceType = typeof(CommonMessage), Name = "DefaultDiscount")]
            public Nullable<decimal> DefaultDiscount { get; set; }
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (CustomRepository.IsCustomerNameExists(CustomerName, CustomerId))
            {
                var fieldName = new[] { "CustomerName" };
                yield return new ValidationResult("Customer Name is Already Exists.", fieldName);
            }
        }
    }

    [MetadataType(typeof(Metadata))]
    public partial class tblCustomerContactPersons
    {
        internal class Metadata
        {
            [ScaffoldColumn(false)]
            public int ContactPersonId { get; set; }

            [ScaffoldColumn(false)]
            public int CustomerId { get; set; }

            [Display(ResourceType = typeof(CommonMessage), Name = "ContactPersonName", Order = 1)]
            [Required(ErrorMessageResourceName = "NameRequired", ErrorMessageResourceType = typeof(CommonMessage))]
            [StringLength(50, ErrorMessageResourceName = "NameLength", ErrorMessageResourceType = typeof(CommonMessage))]
            public string ContactPersonName { get; set; }

            [Display(Name = "Contact Number", Order = 2)]
            [Required(ErrorMessageResourceName = "ContactNoRequired", ErrorMessageResourceType = typeof(CommonMessage))]
            [StringLength(10, ErrorMessageResourceName = "PhoneLength", ErrorMessageResourceType = typeof(CommonMessage))]
            [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid phone number")]
            public string ContactNo { get; set; }

            [DataType(DataType.EmailAddress, ErrorMessageResourceName = "InvalidEmail", ErrorMessageResourceType = typeof(CommonMessage))]
            [Display(ResourceType = typeof(CommonMessage), Name = "Email", Order = 3)]
            [Required(ErrorMessageResourceName = "EmailRequired", ErrorMessageResourceType = typeof(CommonMessage))]
            [UIHint("Email")]
            public string ContactPersonEmail { get; set; }

            [Display(ResourceType = typeof(CommonMessage), Name = "ContactPersonPosistion", Order = 4)]
            public string ContactPersonPosistion { get; set; }


        }
    }

    [MetadataType(typeof(Metadata))]
    public partial class tblMachineParts
    {
        internal class Metadata
        {
            [ScaffoldColumn(false)]
            public int MachinePartId { get; set; }

            [Display(ResourceType = typeof(CommonMessage), Name = "ProductValue")]
            [Required(ErrorMessageResourceName = "ProductValueRequired", ErrorMessageResourceType = typeof(CommonMessage))]
            [StringLength(50, ErrorMessageResourceName = "ProductValueLength", ErrorMessageResourceType = typeof(CommonMessage))]
            public string ProductValue { get; set; }

            [Display(ResourceType = typeof(CommonMessage), Name = "AlternateProductValue")]
            [StringLength(50, ErrorMessageResourceName = "ProductValueLength", ErrorMessageResourceType = typeof(CommonMessage))]
            public string AlternateProductValue { get; set; }

            [Display(ResourceType = typeof(CommonMessage), Name = "Description")]
            public string Description { get; set; }

            [Display(ResourceType = typeof(CommonMessage), Name = "IPL")]
            [Required(ErrorMessageResourceName = "IPLRequired", ErrorMessageResourceType = typeof(CommonMessage))]
            [StringLength(15, ErrorMessageResourceName = "IPLLength", ErrorMessageResourceType = typeof(CommonMessage))]
            public string IPL { get; set; }

            [Display(ResourceType = typeof(CommonMessage), Name = "HSNCode")]
            [Required(ErrorMessageResourceName = "HSNCodeRequired", ErrorMessageResourceType = typeof(CommonMessage))]
            [StringLength(15, ErrorMessageResourceName = "HSNCodeLength", ErrorMessageResourceType = typeof(CommonMessage))]
            public string HSNCode { get; set; }

            [Display(ResourceType = typeof(CommonMessage), Name = "STDPrice")]
            [UIHint("NumericTextbox")]
            public Nullable<decimal> STDPrice { get; set; }

            [Display(ResourceType = typeof(CommonMessage), Name = "ProductTypeName")]
            [Required(ErrorMessageResourceName = "ProductTypeNameRequired", ErrorMessageResourceType = typeof(CommonMessage))]
            [StringLength(50, ErrorMessageResourceName = "ProductTypeNameLength", ErrorMessageResourceType = typeof(CommonMessage))]
            public string ProductTypeName { get; set; }

            [Display(ResourceType = typeof(CommonMessage), Name = "MachineType")]
            [Required(ErrorMessageResourceName = "MachineTypeRequired", ErrorMessageResourceType = typeof(CommonMessage))]
            [UIHint("GridForeignKey")]
            public Nullable<int> MachineTypeId { get; set; }

            [Display(ResourceType = typeof(CommonMessage), Name = "WarrantyPeriod")]
            //[Required(ErrorMessageResourceName = "WarrantyPeriodRequired", ErrorMessageResourceType = typeof(CommonMessage))]
            [UIHint("IntNumericTextBox")]
            public Nullable<int> WarrantyPeriod { get; set; }

            [ScaffoldColumn(false)]
            public Nullable<int> CreatedBy { get; set; }

            [ScaffoldColumn(false)]
            public Nullable<System.DateTime> CreatedDate { get; set; }

            [ScaffoldColumn(false)]
            public Nullable<int> ModifiedBy { get; set; }

            [ScaffoldColumn(false)]
            public Nullable<System.DateTime> ModifiedDate { get; set; }
        }
    }

    [MetadataType(typeof(Metadata))]
    public partial class tblPreCommissioning
    {
        internal class Metadata
        {
            [ScaffoldColumn(false)]
            public int PreCommissioningId { get; set; }

            [Display(ResourceType = typeof(CommonMessage), Name = "CustomerName")]
            [Required(ErrorMessageResourceName = "CustomerRequired", ErrorMessageResourceType = typeof(CommonMessage))]
            public int CustomerId { get; set; }

            [Display(ResourceType = typeof(CommonMessage), Name = "ContactPersonName")]
            [Required(ErrorMessageResourceName = "ContactPersonRequired", ErrorMessageResourceType = typeof(CommonMessage))]
            public int ContactPersonId { get; set; }

            //[Display(ResourceType = typeof(CommonMessage), Name = "MachineType")]
            //[Required(ErrorMessageResourceName = "MachineTypeRequired", ErrorMessageResourceType = typeof(CommonMessage))]
            //public int MachineTypeId { get; set; }

            //[Display(ResourceType = typeof(CommonMessage), Name = "MachineModel")]
            //[Required(ErrorMessageResourceName = "MachineModelRequired", ErrorMessageResourceType = typeof(CommonMessage))]
            //public int MachineModeld { get; set; }

            //[Display(ResourceType = typeof(CommonMessage), Name = "MachineSerialNo")]
            //[Required(ErrorMessageResourceName = "MachineSerialNoRequired", ErrorMessageResourceType = typeof(CommonMessage))]
            //public string MachineSerialNo { get; set; }

            //[Display(ResourceType = typeof(CommonMessage), Name = "AHMNo")]
            ////[Required(ErrorMessageResourceName = "AHMNoRequired", ErrorMessageResourceType = typeof(CommonMessage))]
            //public string AHMNo { get; set; }

            //[Display(ResourceType = typeof(CommonMessage), Name = "DispatchDate")]
            //public Nullable<System.DateTime> DispatchDate { get; set; }

            [ScaffoldColumn(false)]
            public Nullable<int> CreatedBy { get; set; }

            [ScaffoldColumn(false)]
            public Nullable<System.DateTime> CreatedDate { get; set; }

            [ScaffoldColumn(false)]
            public Nullable<int> ModifiedBy { get; set; }

            [ScaffoldColumn(false)]
            public Nullable<System.DateTime> ModifiedDate { get; set; }

            [ScaffoldColumn(false)]
            public Nullable<bool> IsCommissioningDone { get; set; }
        }
    }

    [MetadataType(typeof(Metadata))]
    public partial class tblPreCommissioningMachine : IValidatableObject
    {
        internal class Metadata
        {
            [ScaffoldColumn(false)]
            public int PCMachined { get; set; }

            [ScaffoldColumn(false)]
            public int PreCommissioningId { get; set; }

            [Display(ResourceType = typeof(CommonMessage), Name = "MachineType")]
            [Required(ErrorMessageResourceName = "MachineTypeRequired", ErrorMessageResourceType = typeof(CommonMessage))]
            public int MachineTypeId { get; set; }

            [Display(ResourceType = typeof(CommonMessage), Name = "MachineModel")]
            [Required(ErrorMessageResourceName = "MachineModelRequired", ErrorMessageResourceType = typeof(CommonMessage))]
            public int MachineModeld { get; set; }

            [Display(ResourceType = typeof(CommonMessage), Name = "MachineSerialNo")]
            [Required(ErrorMessageResourceName = "MachineSerialNoRequired", ErrorMessageResourceType = typeof(CommonMessage))]
            public string MachineSerialNo { get; set; }

            [DataType(DataType.Date)]
            [Required(ErrorMessageResourceName = "DispatchDateRequired", ErrorMessageResourceType = typeof(CommonMessage))]
            [Display(ResourceType = typeof(CommonMessage), Name = "DispatchDate")]
            public System.DateTime DispatchDate { get; set; }

            [Display(ResourceType = typeof(CommonMessage), Name = "AHMNo")]
            //[Required(ErrorMessageResourceName = "AHMNoRequired", ErrorMessageResourceType = typeof(CommonMessage))]
            public string AHMNo { get; set; }

            [ScaffoldColumn(false)]
            public Nullable<int> CreatedBy { get; set; }

            [ScaffoldColumn(false)]
            public Nullable<System.DateTime> CreatedDate { get; set; }

            [ScaffoldColumn(false)]
            public Nullable<int> ModifiedBy { get; set; }

            [ScaffoldColumn(false)]
            public Nullable<System.DateTime> ModifiedDate { get; set; }
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (CustomRepository.IsPCMachineSerialAlreadyExists(PreCommissioningId, MachineSerialNo, PCMachined))
            {
                var fieldName = new[] { "MachineSerialNo" };
                yield return new ValidationResult("Machine Serial No is Already Exists.", fieldName);
            }
        }
    }

    [MetadataType(typeof(Metadata))]
    public partial class tblPreCommissioningDetail
    {
        [Display(Name = "Machine List")]
        public List<int> PCMachineIdList { get; set; }

        [Display(Name = "Accessories List")]
        public List<int> PCAccessoryIdList { get; set; }

        internal class Metadata
        {
            [ScaffoldColumn(false)]
            public int PCDetailId { get; set; }

            [ScaffoldColumn(false)]
            public int PreCommissionId { get; set; }

            public Nullable<int> PCMachineId { get; set; }
            public Nullable<int> PCAccesseriesId { get; set; }

            [Display(ResourceType = typeof(CommonMessage), Name = "PreCommisoningDate", Order = 1)]
            [Required(ErrorMessageResourceName = "PreCommisoningDateRequired", ErrorMessageResourceType = typeof(CommonMessage))]
            //[UIHint("Date")]
            public System.DateTime PreCommisoningDate { get; set; }

            //[UIHint("GridForeignKey")]
            [Display(ResourceType = typeof(CommonMessage), Name = "ServiceEngineer", Order = 2)]
            [Required(ErrorMessageResourceName = "ServiceEngineerRequired", ErrorMessageResourceType = typeof(CommonMessage))]
            public int ServiceEngineerId { get; set; }

            //[UIHint("TextArea")]
            [Display(ResourceType = typeof(CommonMessage), Name = "PrecommisioningRemark", Order = 3)]
            public string PrecommisioningRemark { get; set; }

            [ScaffoldColumn(false)]
            //[Display(ResourceType = typeof(CommonMessage), Name = "IsCommisioning", Order = 4)]
            public bool IsCommisioning { get; set; }

            [ScaffoldColumn(false)]
            public Nullable<int> CreatedBy { get; set; }

            [ScaffoldColumn(false)]
            public Nullable<System.DateTime> CreatedDate { get; set; }

            [ScaffoldColumn(false)]
            public Nullable<int> ModifiedBy { get; set; }

            [ScaffoldColumn(false)]
            public Nullable<System.DateTime> ModifiedDate { get; set; }
        }
    }

    [MetadataType(typeof(Metadata))]
    public partial class tblMachineModels
    {
        internal class Metadata
        {
            [ScaffoldColumn(false)]
            public int MachineModelId { get; set; }

            [Display(ResourceType = typeof(CommonMessage), Name = "MachineName", Order = 1)]
            [Required(ErrorMessageResourceName = "MachineNameRequired", ErrorMessageResourceType = typeof(CommonMessage))]
            [StringLength(50, ErrorMessageResourceName = "MachineNameLength", ErrorMessageResourceType = typeof(CommonMessage))]
            public string MachineName { get; set; }

            [Display(ResourceType = typeof(CommonMessage), Name = "MachineType", Order = 2)]
            [Required(ErrorMessageResourceName = "MachineTypeRequired", ErrorMessageResourceType = typeof(CommonMessage))]
            [UIHint("GridForeignKey")]
            public int MachineTypeId { get; set; }

            [Display(ResourceType = typeof(CommonMessage), Name = "WarrantyPeriod", Order = 3)]
            [Required(ErrorMessageResourceName = "WarrantyPeriod", ErrorMessageResourceType = typeof(CommonMessage))]
            [UIHint("IntNumericTextBox")]
            public int WarrantyPeriod { get; set; }

            [Display(ResourceType = typeof(CommonMessage), Name = "Status", Order = 4)]
            public bool IsActive { get; set; }

            [ScaffoldColumn(false)]
            public Nullable<int> CreatedBy { get; set; }

            [ScaffoldColumn(false)]
            public Nullable<System.DateTime> CreatedDate { get; set; }

            [ScaffoldColumn(false)]
            public Nullable<int> ModifiedBy { get; set; }

            [ScaffoldColumn(false)]
            public Nullable<System.DateTime> ModifiedDate { get; set; }
        }
    }

    [MetadataType(typeof(Metadata))]
    public partial class tblMachineAccessories
    {
        internal class Metadata
        {
            [ScaffoldColumn(false)]
            public int MachineAccessoriesId { get; set; }

            [Display(ResourceType = typeof(CommonMessage), Name = "AccessoriesName", Order = 1)]
            [Required(ErrorMessageResourceName = "AccessoriesNameRequired", ErrorMessageResourceType = typeof(CommonMessage))]
            [StringLength(50, ErrorMessageResourceName = "AccessoriesNameLength", ErrorMessageResourceType = typeof(CommonMessage))]
            public string AccessoriesName { get; set; }

            [Display(ResourceType = typeof(CommonMessage), Name = "AccessoriesType", Order = 2)]
            [Required(ErrorMessageResourceName = "AccessoriesTypeRequired", ErrorMessageResourceType = typeof(CommonMessage))]
            [UIHint("GridForeignKey")]
            public int AccessoriesTypeId { get; set; }

            [Display(ResourceType = typeof(CommonMessage), Name = "Status", Order = 3)]
            public bool IsActive { get; set; }

            [ScaffoldColumn(false)]
            public Nullable<int> CreatedBy { get; set; }

            [ScaffoldColumn(false)]
            public Nullable<System.DateTime> CreatedDate { get; set; }

            [ScaffoldColumn(false)]
            public Nullable<int> ModifiedBy { get; set; }

            [ScaffoldColumn(false)]
            public Nullable<System.DateTime> ModifiedDate { get; set; }
        }
    }

    [MetadataType(typeof(Metadata))]
    public partial class tblPreCommissioningAccessories : IValidatableObject
    {
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (CustomRepository.IsPCAccessorySerialAlreadyExists(PreCommissioningId, AccessoriesSerialNo, PCAccessoriesId))
            {
                var fieldName = new[] { "AccessoriesSerialNo" };
                yield return new ValidationResult("Accessories Serial No is already exists.", fieldName);
            }
        }

        internal class Metadata
        {
            [ScaffoldColumn(false)]
            public int PCAccessoriesId { get; set; }

            [ScaffoldColumn(false)]
            public int PreCommissioningId { get; set; }

            [Display(ResourceType = typeof(CommonMessage), Name = "AccessoriesSerialNo", Order = 3)]
            [Required(ErrorMessageResourceName = "AccessoriesSerialNoRequired", ErrorMessageResourceType = typeof(CommonMessage))]
            [StringLength(50, ErrorMessageResourceName = "AccessoriesSerialNoLength", ErrorMessageResourceType = typeof(CommonMessage))]
            public string AccessoriesSerialNo { get; set; }

            [Display(ResourceType = typeof(CommonMessage), Name = "AccessoriesType", Order = 1)]
            [Required(ErrorMessageResourceName = "AccessoriesTypeRequired", ErrorMessageResourceType = typeof(CommonMessage))]
            [UIHint("GridForeignKey")]
            public int AccessoriesTypeId { get; set; }

            [Display(ResourceType = typeof(CommonMessage), Name = "MachineAccessories", Order = 2)]
            [Required(ErrorMessageResourceName = "MachineAccessoriesRequired", ErrorMessageResourceType = typeof(CommonMessage))]
            [UIHint("GridForeignKey")]
            public int MachineAccessoriesId { get; set; }

            [Required(ErrorMessageResourceName = "DispatchDateRequired", ErrorMessageResourceType = typeof(CommonMessage))]
            [Display(ResourceType = typeof(CommonMessage), Name = "DispatchDate")]
            public System.DateTime DispatchDate { get; set; }

            [ScaffoldColumn(false)]
            public Nullable<int> CreatedBy { get; set; }

            [ScaffoldColumn(false)]
            public Nullable<System.DateTime> CreatedDate { get; set; }

            [ScaffoldColumn(false)]
            public Nullable<int> ModifiedBy { get; set; }

            [ScaffoldColumn(false)]
            public Nullable<System.DateTime> ModifiedDate { get; set; }
        }
    }

    [MetadataType(typeof(Metadata))]
    public partial class tblMachinePartsQuotation
    {
        internal class Metadata
        {
            [ScaffoldColumn(false)]
            public int MachinePartsQuotationId { get; set; }

            [Display(ResourceType = typeof(CommonMessage), Name = "QuotationNo")]
            public string QuotationNo { get; set; }

            [Display(ResourceType = typeof(CommonMessage), Name = "QuotationDate")]
            [Required(ErrorMessageResourceName = "QuotationDateRequired", ErrorMessageResourceType = typeof(CommonMessage))]
            public System.DateTime QuotationDate { get; set; }

            [Display(ResourceType = typeof(CommonMessage), Name = "CustomerName")]
            [Required(ErrorMessageResourceName = "CustomerRequired", ErrorMessageResourceType = typeof(CommonMessage))]
            public int CustomerId { get; set; }

            [Display(ResourceType = typeof(CommonMessage), Name = "ContactPersonName")]
            [Required(ErrorMessageResourceName = "ContactPersonRequired", ErrorMessageResourceType = typeof(CommonMessage))]
            public int CustomerContactPId { get; set; }

            [Display(Name = "Contact Person Contact Number")]
            public string CustomerContactPContactNo { get; set; }

            [Display(ResourceType = typeof(CommonMessage), Name = "InquiryNo")]
            //[Required(ErrorMessageResourceName = "InquiryNoRequired", ErrorMessageResourceType = typeof(CommonMessage))]
            [StringLength(50, ErrorMessageResourceName = "InquiryNoLength", ErrorMessageResourceType = typeof(CommonMessage))]
            public string InquiryNo { get; set; }

            [Display(ResourceType = typeof(CommonMessage), Name = "InquiryDate")]
            [Required(ErrorMessageResourceName = "InquiryDateRequired", ErrorMessageResourceType = typeof(CommonMessage))]
            public Nullable<System.DateTime> InquiryDate { get; set; }

            [Display(ResourceType = typeof(CommonMessage), Name = "PaymentTerms")]
            //[Required(ErrorMessageResourceName = "PaymentTermsRequired", ErrorMessageResourceType = typeof(CommonMessage))]
            public string PaymentTerms { get; set; }

            [Display(ResourceType = typeof(CommonMessage), Name = "DeliveryWeeks")]
            [Required(ErrorMessageResourceName = "DeliveryWeeksRequired", ErrorMessageResourceType = typeof(CommonMessage))]
            public int DeliveryWeeks { get; set; }

            [Display(ResourceType = typeof(CommonMessage), Name = "Insurance")]
            [StringLength(100, ErrorMessageResourceName = "InsuranceLength", ErrorMessageResourceType = typeof(CommonMessage))]
            public string Insurance { get; set; }

            [Display(ResourceType = typeof(CommonMessage), Name = "ValidityDays")]
            [Required(ErrorMessageResourceName = "ValidityDaysRequired", ErrorMessageResourceType = typeof(CommonMessage))]
            public int ValidityDays { get; set; }

            [DataType(DataType.EmailAddress, ErrorMessageResourceName = "InvalidEmail", ErrorMessageResourceType = typeof(CommonMessage))]
            [Display(ResourceType = typeof(CommonMessage), Name = "Email")]
            public string Email { get; set; }

            [Display(ResourceType = typeof(CommonMessage), Name = "ReportServiceNo")]
            public string ReportServiceNo { get; set; }

            [Display(ResourceType = typeof(CommonMessage), Name = "Freight")]
            public Nullable<decimal> FreightAmount { get; set; }

            public Nullable<decimal> TotalFinalAmount { get; set; }
            public Nullable<decimal> QuotationAmount { get; set; }

            public Nullable<decimal> FreightPercentage { get; set; }
            public Nullable<decimal> TotalFreightAmount { get; set; }
            public string Remarks { get; set; }

            [Display(ResourceType = typeof(CommonMessage), Name = "ServiceEngineer")]
            public Nullable<int> ServiceEngineerId { get; set; }

            [ScaffoldColumn(false)]
            public Nullable<int> SequenceNo { get; set; }

            [ScaffoldColumn(false)]
            public Nullable<int> CreatedBy { get; set; }

            [ScaffoldColumn(false)]
            public Nullable<System.DateTime> CreatedDate { get; set; }

            [ScaffoldColumn(false)]
            public Nullable<int> ModifiedBy { get; set; }

            [ScaffoldColumn(false)]
            public Nullable<System.DateTime> ModifiedDate { get; set; }
        }
    }

    [MetadataType(typeof(Metadata))]
    public partial class tblMachinePartsQuotationDetail : IValidatableObject
    {
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (MachinePartsId == 0)
            {
                var fieldName = new[] { "MachinePartsId" };
                yield return new ValidationResult("Machine Parts is required.", fieldName);
            }

            if (FinalAmount == 0)
            {
                var fieldName = new[] { "FinalAmount" };
                yield return new ValidationResult("Final Amount is required.", fieldName);
            }

            if (PartsQuantity == 0)
            {
                var fieldName = new[] { "PartsQuantity" };
                yield return new ValidationResult("Parts Quantity is required.", fieldName);
            }

            if (UnitPrice == 0)
            {
                var fieldName = new[] { "UnitPrice" };
                yield return new ValidationResult("Unit Price is required.", fieldName);
            }
        }

        internal class Metadata
        {
            [ScaffoldColumn(false)]
            public int MPQDetailId { get; set; }

            [Display(ResourceType = typeof(CommonMessage), Name = "MachineType")]
            [Required(ErrorMessageResourceName = "MachineTypeRequired", ErrorMessageResourceType = typeof(CommonMessage))]
            public int MachineTypeId { get; set; }

            [Display(ResourceType = typeof(CommonMessage), Name = "MachineModel")]
            [Required(ErrorMessageResourceName = "MachineModelRequired", ErrorMessageResourceType = typeof(CommonMessage))]
            public int MachineModelId { get; set; }

            [Display(ResourceType = typeof(CommonMessage), Name = "MachineSerialNo")]
            [Required(ErrorMessageResourceName = "MachineSerialNoRequired", ErrorMessageResourceType = typeof(CommonMessage))]
            public string MachineModelSerialNo { get; set; }

            [Display(ResourceType = typeof(CommonMessage), Name = "MachineParts")]
            [Required(ErrorMessageResourceName = "MachinePartsRequired", ErrorMessageResourceType = typeof(CommonMessage))]
            public int MachinePartsId { get; set; }

            [Display(ResourceType = typeof(CommonMessage), Name = "MachinePartsNo")]
            public string MachinePartsNo { get; set; }

            [Display(ResourceType = typeof(CommonMessage), Name = "MachinePartDescription")]
            public string MachinePartDescription { get; set; }

            [Display(ResourceType = typeof(CommonMessage), Name = "PartsHSNCode")]
            public string PartsHSNCode { get; set; }

            [Display(ResourceType = typeof(CommonMessage), Name = "PartsQuantity")]
            [Required(ErrorMessageResourceName = "PartsQuantityRequired", ErrorMessageResourceType = typeof(CommonMessage))]
            public int PartsQuantity { get; set; }

            [Display(ResourceType = typeof(CommonMessage), Name = "UnitPrice")]
            [Required(ErrorMessageResourceName = "UnitPriceRequired", ErrorMessageResourceType = typeof(CommonMessage))]
            public decimal UnitPrice { get; set; }

            [Display(ResourceType = typeof(CommonMessage), Name = "PAndFPercentage")]
            public Nullable<decimal> PAndFPercentage { get; set; }

            [Display(ResourceType = typeof(CommonMessage), Name = "ProfitMarginPercentage")]
            public Nullable<decimal> ProfitMarginPercentage { get; set; }

            [Required(ErrorMessageResourceName = "TotalPriceRequired", ErrorMessageResourceType = typeof(CommonMessage))]
            [Display(ResourceType = typeof(CommonMessage), Name = "TotalPrice")]
            public decimal TotalPrice { get; set; }

            [Required(ErrorMessageResourceName = "FinalAmountRequired", ErrorMessageResourceType = typeof(CommonMessage))]
            [Display(ResourceType = typeof(CommonMessage), Name = "FinalAmount")]
            public decimal FinalAmount { get; set; }

            [Display(ResourceType = typeof(CommonMessage), Name = "DiscountPercentage")]
            public decimal DiscountPercentage { get; set; }

            [Display(ResourceType = typeof(CommonMessage), Name = "TaxablePrice")]
            public decimal TaxablePrice { get; set; }

            [Display(ResourceType = typeof(CommonMessage), Name = "GSTPercentage")]
            [Required(ErrorMessageResourceName = "GSTPercentageRequired", ErrorMessageResourceType = typeof(CommonMessage))]
            public Nullable<decimal> GSTPercentage { get; set; }

            [Display(ResourceType = typeof(CommonMessage), Name = "GSTAmount")]
            public Nullable<decimal> GSTAmount { get; set; }

            [Display(ResourceType = typeof(CommonMessage), Name = "SellingPrice")]
            public Nullable<decimal> SellingPrice { get; set; }

            [ScaffoldColumn(false)]
            public Nullable<int> CreatedBy { get; set; }

            [ScaffoldColumn(false)]
            public Nullable<System.DateTime> CreatedDate { get; set; }

            [ScaffoldColumn(false)]
            public Nullable<int> ModifiedBy { get; set; }

            [ScaffoldColumn(false)]
            public Nullable<System.DateTime> ModifiedDate { get; set; }
        }
    }

    [MetadataType(typeof(Metadata))]
    public partial class tblAMCQuotation : IValidatableObject
    {
        internal class Metadata
        {
            [ScaffoldColumn(false)]
            public int Id { get; set; }

            [ScaffoldColumn(false)]
            public int CommissioningId { get; set; }

            [Display(ResourceType = typeof(CommonMessage), Name = "MachineType")]
            public int MachineTypeId { get; set; }

            [Display(ResourceType = typeof(CommonMessage), Name = "MachineModel")]
            public int MachineModelId { get; set; }

            [Display(ResourceType = typeof(CommonMessage), Name = "MachineSerialNo")]
            public string MachineSerialNo { get; set; }

            [Display(ResourceType = typeof(CommonMessage), Name = "CustomerName")]
            public int CustomerId { get; set; }

            [Display(ResourceType = typeof(CommonMessage), Name = "AMCQuotationDate")]
            public System.DateTime QuotationDate { get; set; }

            [Display(ResourceType = typeof(CommonMessage), Name = "ActualAmount")]
            public decimal ActualAmount { get; set; }

            [Display(ResourceType = typeof(CommonMessage), Name = "GSTPercentage")]
            public decimal GSTPercentage { get; set; }

            [Display(ResourceType = typeof(CommonMessage), Name = "GSTAmount")]
            public decimal GSTAmount { get; set; }

            [Display(ResourceType = typeof(CommonMessage), Name = "TotalAmount")]
            public decimal TotalAmount { get; set; }
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (CustomRepository.IsAMCQuotationWithSameAmountExists(MachineTypeId, MachineModelId, MachineSerialNo, CustomerId, ActualAmount, GSTPercentage, Id))
            {
                var fieldName = new[] { "ActualAmount" };
                yield return new ValidationResult("AMC Quotation with same Amount & GST Percentage is already exists.", fieldName);
            }
        }
    }

    [MetadataType(typeof(Metadata))]
    public partial class tblCommissioning
    {
        public bool IsWarrantyPeriodChange { get; set; }

        internal class Metadata
        {
            [ScaffoldColumn(false)]
            public int CommissioningId { get; set; }

            [Display(ResourceType = typeof(CommonMessage), Name = "MachineType")]
            public int MachineTypeId { get; set; }

            [Display(ResourceType = typeof(CommonMessage), Name = "MachineModel")]
            public int MachineModelId { get; set; }

            [Display(ResourceType = typeof(CommonMessage), Name = "MachineSerialNo")]
            public string MachineSerialNo { get; set; }

            [Display(ResourceType = typeof(CommonMessage), Name = "ServiceEngineer")]
            public int ServiceEngineerId { get; set; }

            [Display(ResourceType = typeof(CommonMessage), Name = "CommissioningDate")]
            public System.DateTime CommissioningDate { get; set; }

            [Display(ResourceType = typeof(CommonMessage), Name = "WarrantyExpireDate")]
            public System.DateTime WarrantyExpireDate { get; set; }

            [Display(ResourceType = typeof(CommonMessage), Name = "WarrantyPeriod")]
            public int WarrantyPeriod { get; set; }

            [Display(ResourceType = typeof(CommonMessage), Name = "Remark")]
            public string Remark { get; set; }

            [Display(ResourceType = typeof(CommonMessage), Name = "CommissioningFileName")]
            public string CommissioningFileName { get; set; }

            [Display(ResourceType = typeof(CommonMessage), Name = "CustomerName")]
            public Nullable<int> CustomerId { get; set; }

            [Display(ResourceType = typeof(CommonMessage), Name = "ContactPersonName")]
            public Nullable<int> ContactPersonId { get; set; }

            [Display(ResourceType = typeof(CommonMessage), Name = "ContactNo")]
            public string ContactPersonContactNo { get; set; }

            [Display(ResourceType = typeof(CommonMessage), Name = "ReportServiceNo")]
            public string ReportServiceNo { get; set; }
        }

        //public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        //{
        //    if (CustomRepository.IsCommissioningReportServiceNoExists(CommissioningId, ReportServiceNo))
        //    {
        //        var fieldName = new[] { "ReportServiceNo" };
        //        yield return new ValidationResult("Report Service No is already exists.", fieldName);
        //    }
        //}
    }

    [MetadataType(typeof(Metadata))]
    public partial class tblPerformaInvoice
    {
        internal class Metadata
        {
            [ScaffoldColumn(false)]
            public int PerformaInvoiceId { get; set; }

            [ScaffoldColumn(false)]
            public Nullable<int> MPQuotationId { get; set; }

            [Display(ResourceType = typeof(CommonMessage), Name = "QuotationNo")]
            public string QuotationNo { get; set; }

            [Display(ResourceType = typeof(CommonMessage), Name = "QuotationDate")]
            [Required(ErrorMessageResourceName = "QuotationDateRequired", ErrorMessageResourceType = typeof(CommonMessage))]
            public System.DateTime QuotationDate { get; set; }

            [Display(ResourceType = typeof(CommonMessage), Name = "CustomerName")]
            [Required(ErrorMessageResourceName = "CustomerRequired", ErrorMessageResourceType = typeof(CommonMessage))]
            public int CustomerId { get; set; }

            [Display(ResourceType = typeof(CommonMessage), Name = "ContactPersonName")]
            [Required(ErrorMessageResourceName = "ContactPersonRequired", ErrorMessageResourceType = typeof(CommonMessage))]
            public int CustomerContactPId { get; set; }

            [Display(Name = "Contact Person Contact Number")]
            public string CustomerContactPContactNo { get; set; }

            [Display(ResourceType = typeof(CommonMessage), Name = "InquiryNo")]
            //[Required(ErrorMessageResourceName = "InquiryNoRequired", ErrorMessageResourceType = typeof(CommonMessage))]
            [StringLength(50, ErrorMessageResourceName = "InquiryNoLength", ErrorMessageResourceType = typeof(CommonMessage))]
            public string InquiryNo { get; set; }

            [Display(ResourceType = typeof(CommonMessage), Name = "InquiryDate")]
            [Required(ErrorMessageResourceName = "InquiryDateRequired", ErrorMessageResourceType = typeof(CommonMessage))]
            public Nullable<System.DateTime> InquiryDate { get; set; }

            [Display(ResourceType = typeof(CommonMessage), Name = "PaymentTerms")]
            //[Required(ErrorMessageResourceName = "PaymentTermsRequired", ErrorMessageResourceType = typeof(CommonMessage))]
            public string PaymentTerms { get; set; }

            [Display(ResourceType = typeof(CommonMessage), Name = "DeliveryWeeks")]
            [Required(ErrorMessageResourceName = "DeliveryWeeksRequired", ErrorMessageResourceType = typeof(CommonMessage))]
            public int DeliveryWeeks { get; set; }

            [Display(ResourceType = typeof(CommonMessage), Name = "Insurance")]
            [StringLength(100, ErrorMessageResourceName = "InsuranceLength", ErrorMessageResourceType = typeof(CommonMessage))]
            public string Insurance { get; set; }

            [Display(ResourceType = typeof(CommonMessage), Name = "ValidityDays")]
            [Required(ErrorMessageResourceName = "ValidityDaysRequired", ErrorMessageResourceType = typeof(CommonMessage))]
            public int ValidityDays { get; set; }

            [DataType(DataType.EmailAddress, ErrorMessageResourceName = "InvalidEmail", ErrorMessageResourceType = typeof(CommonMessage))]
            [Display(ResourceType = typeof(CommonMessage), Name = "Email")]
            public string Email { get; set; }

            [Display(ResourceType = typeof(CommonMessage), Name = "ReportServiceNo")]
            public string ReportServiceNo { get; set; }

            [Display(ResourceType = typeof(CommonMessage), Name = "Freight")]
            public Nullable<decimal> FreightAmount { get; set; }

            public Nullable<decimal> TotalFinalAmount { get; set; }
            public Nullable<decimal> QuotationAmount { get; set; }

            public string Remarks { get; set; }

            [Display(ResourceType = typeof(CommonMessage), Name = "ServiceEngineer")]
            public Nullable<int> ServiceEngineerId { get; set; }

            public Nullable<decimal> FreightPercentage { get; set; }
            public Nullable<decimal> TotalFreightAmount { get; set; }

            [ScaffoldColumn(false)]
            public Nullable<int> SequenceNo { get; set; }

            [ScaffoldColumn(false)]
            public Nullable<int> CreatedBy { get; set; }

            [ScaffoldColumn(false)]
            public Nullable<System.DateTime> CreatedDate { get; set; }

            [ScaffoldColumn(false)]
            public Nullable<int> ModifiedBy { get; set; }

            [ScaffoldColumn(false)]
            public Nullable<System.DateTime> ModifiedDate { get; set; }
        }
    }

    [MetadataType(typeof(Metadata))]
    public partial class tblPerformaInvoiceDetail : IValidatableObject
    {
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (MachinePartsId == 0)
            {
                var fieldName = new[] { "MachinePartsId" };
                yield return new ValidationResult("Machine Parts is required.", fieldName);
            }

            if (FinalAmount == 0)
            {
                var fieldName = new[] { "FinalAmount" };
                yield return new ValidationResult("Final Amount is required.", fieldName);
            }

            if (PartsQuantity == 0)
            {
                var fieldName = new[] { "PartsQuantity" };
                yield return new ValidationResult("Parts Quantity is required.", fieldName);
            }

            if (UnitPrice == 0)
            {
                var fieldName = new[] { "UnitPrice" };
                yield return new ValidationResult("Unit Price is required.", fieldName);
            }
        }

        internal class Metadata
        {
            [ScaffoldColumn(false)]
            public int PIDetailId { get; set; }

            public int PerformaInvoiceId { get; set; }
            public int MPQDetailId { get; set; }

            [Display(ResourceType = typeof(CommonMessage), Name = "MachineType")]
            [Required(ErrorMessageResourceName = "MachineTypeRequired", ErrorMessageResourceType = typeof(CommonMessage))]
            public int MachineTypeId { get; set; }

            [Display(ResourceType = typeof(CommonMessage), Name = "MachineModel")]
            [Required(ErrorMessageResourceName = "MachineModelRequired", ErrorMessageResourceType = typeof(CommonMessage))]
            public int MachineModelId { get; set; }

            [Display(ResourceType = typeof(CommonMessage), Name = "MachineSerialNo")]
            [Required(ErrorMessageResourceName = "MachineSerialNoRequired", ErrorMessageResourceType = typeof(CommonMessage))]
            public string MachineModelSerialNo { get; set; }

            [Display(ResourceType = typeof(CommonMessage), Name = "MachineParts")]
            [Required(ErrorMessageResourceName = "MachinePartsRequired", ErrorMessageResourceType = typeof(CommonMessage))]
            public int MachinePartsId { get; set; }

            [Display(ResourceType = typeof(CommonMessage), Name = "MachinePartsNo")]
            public string MachinePartsNo { get; set; }

            [Display(ResourceType = typeof(CommonMessage), Name = "MachinePartDescription")]
            public string MachinePartDescription { get; set; }

            [Display(ResourceType = typeof(CommonMessage), Name = "PartsHSNCode")]
            public string PartsHSNCode { get; set; }

            [Display(ResourceType = typeof(CommonMessage), Name = "PartsQuantity")]
            [Required(ErrorMessageResourceName = "PartsQuantityRequired", ErrorMessageResourceType = typeof(CommonMessage))]
            public int PartsQuantity { get; set; }

            [Display(ResourceType = typeof(CommonMessage), Name = "UnitPrice")]
            [Required(ErrorMessageResourceName = "UnitPriceRequired", ErrorMessageResourceType = typeof(CommonMessage))]
            public decimal UnitPrice { get; set; }

            [Display(ResourceType = typeof(CommonMessage), Name = "PAndFPercentage")]
            public Nullable<decimal> PAndFPercentage { get; set; }

            [Display(ResourceType = typeof(CommonMessage), Name = "ProfitMarginPercentage")]
            public Nullable<decimal> ProfitMarginPercentage { get; set; }

            [Required(ErrorMessageResourceName = "TotalPriceRequired", ErrorMessageResourceType = typeof(CommonMessage))]
            [Display(ResourceType = typeof(CommonMessage), Name = "TotalPrice")]
            public decimal TotalPrice { get; set; }

            [Required(ErrorMessageResourceName = "FinalAmountRequired", ErrorMessageResourceType = typeof(CommonMessage))]
            [Display(ResourceType = typeof(CommonMessage), Name = "FinalAmount")]
            public decimal FinalAmount { get; set; }

            [Display(ResourceType = typeof(CommonMessage), Name = "DiscountPercentage")]
            public decimal DiscountPercentage { get; set; }

            [Display(ResourceType = typeof(CommonMessage), Name = "TaxablePrice")]
            public decimal TaxablePrice { get; set; }

            [Display(ResourceType = typeof(CommonMessage), Name = "GSTPercentage")]
            [Required(ErrorMessageResourceName = "GSTPercentageRequired", ErrorMessageResourceType = typeof(CommonMessage))]
            public Nullable<decimal> GSTPercentage { get; set; }

            [Display(ResourceType = typeof(CommonMessage), Name = "GSTAmount")]
            public Nullable<decimal> GSTAmount { get; set; }

            [Display(ResourceType = typeof(CommonMessage), Name = "SellingPrice")]
            public Nullable<decimal> SellingPrice { get; set; }

            [ScaffoldColumn(false)]
            public Nullable<int> CreatedBy { get; set; }

            [ScaffoldColumn(false)]
            public Nullable<System.DateTime> CreatedDate { get; set; }

            [ScaffoldColumn(false)]
            public Nullable<int> ModifiedBy { get; set; }

            [ScaffoldColumn(false)]
            public Nullable<System.DateTime> ModifiedDate { get; set; }
        }
    }

    [MetadataType(typeof(Metadata))]
    public partial class tblRegion : IValidatableObject
    {
        internal class Metadata
        {
            [ScaffoldColumn(false)]
            public int Id { get; set; }

            [Display(ResourceType = typeof(CommonMessage), Name = "Region", Order = 1)]
            [Required(ErrorMessageResourceName = "RegionNameRequired", ErrorMessageResourceType = typeof(CommonMessage))]
            [StringLength(50, ErrorMessageResourceName = "RegionNameLength", ErrorMessageResourceType = typeof(CommonMessage))]
            public string Name { get; set; }

            [UIHint("GridForeignKey")]
            [Display(ResourceType = typeof(CommonMessage), Name = "City", Order = 2)]
            [Required(ErrorMessageResourceName = "CityRequired", ErrorMessageResourceType = typeof(CommonMessage))]
            public int CityId { get; set; }

            [Display(ResourceType = typeof(CommonMessage), Name = "Status", Order = 3)]
            public bool IsActive { get; set; }
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (CustomRepository.IsRegionNameExists(Name, Id,CityId))
            {
                var fieldName = new[] { "Name" };
                yield return new ValidationResult("Region is Already Exists.", fieldName);
            }
        }

    }

    [MetadataType(typeof(Metadata))]
    public partial class GetPreCommisioningDetailData_Result
    {
        [Display(Name = "Machine List")]
        public List<int> PCMachineIdList { get; set; }

        [Display(Name = "Accessories List")]
        public List<int> PCAccessoryIdList { get; set; }

        internal class Metadata
        {

            public int PCDetailId { get; set; }
            public int PreCommissionId { get; set; }
            public string MachineSerialNo { get; set; }
            public string MachineTypeName { get; set; }
            public string MachineName { get; set; }
            public string AccessoriesSerialNo { get; set; }
            public string AccessoriesTypeName { get; set; }
            public string AccessoriesName { get; set; }
            public Nullable<bool> IsLatest { get; set; }
            public bool IsCommisioning { get; set; }

            [DataType(DataType.Date)]
            [Display(ResourceType = typeof(CommonMessage), Name = "PreCommisoningDate", Order = 1)]
            [Required(ErrorMessageResourceName = "PreCommisoningDateRequired", ErrorMessageResourceType = typeof(CommonMessage))]
            public System.DateTime PreCommisoningDate { get; set; }

            [Display(ResourceType = typeof(CommonMessage), Name = "PrecommisioningRemark", Order = 3)]
            public string PrecommisioningRemark { get; set; }
            
            public string ServiceEngineer { get; set; }
            public Nullable<int> PCAccesseriesId { get; set; }
            public Nullable<int> PCMachineId { get; set; }

            [Display(ResourceType = typeof(CommonMessage), Name = "ServiceEngineer", Order = 2)]
            [Required(ErrorMessageResourceName = "ServiceEngineerRequired", ErrorMessageResourceType = typeof(CommonMessage))]
            public int ServiceEngineerId { get; set; }
        }
    }
}
