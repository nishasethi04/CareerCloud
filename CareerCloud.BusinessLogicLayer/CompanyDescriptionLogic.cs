using CareerCloud.Pocos;
using System;
using CareerCloud.DataAccessLayer;
using System.Collections.Generic;
using System.Text;

namespace CareerCloud.BusinessLogicLayer
{
    public class CompanyDescriptionLogic:BaseLogic<CompanyDescriptionPoco>
            {
        public CompanyDescriptionLogic(IDataRepository<CompanyDescriptionPoco> repository) : base(repository)
        {

        }
        protected override void Verify(CompanyDescriptionPoco[] pocos)
        {
            List<ValidationException> exceptions = new List<ValidationException>();
            foreach(CompanyDescriptionPoco poco in pocos)
            {
                if (String.IsNullOrEmpty(poco.CompanyName))
                {
                    exceptions.Add(new ValidationException(106, "Company Description is required field"));
                }
                else if (poco.CompanyName.Length<3)
                {
                    exceptions.Add(new ValidationException(106, "Company Name must be greater than 2 Characters"));
                }
                if(String.IsNullOrEmpty(poco.CompanyDescription))
                {
                    exceptions.Add(new ValidationException(107, "Company Description is required field"));
                }
                else if(poco.CompanyDescription.Length<3)
                {
                    exceptions.Add(new ValidationException(107, "Company Description must be greater than 2 characters"));
                }

            }
            if(exceptions.Count>0)
            {
                throw new AggregateException(exceptions);
            }
        }
        public override void Update(CompanyDescriptionPoco[] pocos)
        {
            Verify(pocos);
            base.Update(pocos);
        }
        public override void Add(CompanyDescriptionPoco[] pocos)
        {
            Verify(pocos);
            base.Add(pocos);
        }
    }
}
