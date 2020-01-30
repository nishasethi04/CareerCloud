using CareerCloud.DataAccessLayer;
using CareerCloud.Pocos;
using System;
using System.Collections.Generic;
using System.Text;

namespace CareerCloud.BusinessLogicLayer
{
    public class CompanyJobEducationLogic:BaseLogic<CompanyJobEducationPoco>
    {
        public CompanyJobEducationLogic(IDataRepository<CompanyJobEducationPoco> repository) : base(repository)
        {

        }
        protected override void Verify(CompanyJobEducationPoco[] pocos)
        {
            List<ValidationException> exceptions = new List<ValidationException>();
            foreach(CompanyJobEducationPoco poco in pocos)
            {
                if(String.IsNullOrEmpty(poco.Major))
                {
                    exceptions.Add(new ValidationException(200, "Major cannot be empty"));

                }
                else if(poco.Major.Length<2)
                {
                    exceptions.Add(new ValidationException(200,"The length of major must be greater than 2"));
                }
                if(poco.Importance<0)
                {
                    exceptions.Add(new ValidationException(201, "Importance cannot be negative"));
                }
            }
            if(exceptions.Count>0)
            {
                throw new AggregateException(exceptions);
            }
           
        }
        public override void Update(CompanyJobEducationPoco[] pocos)
        {
            Verify(pocos);
            base.Update(pocos);
        }
        public override void Add(CompanyJobEducationPoco[] pocos)
        {
            Verify(pocos);
            base.Add(pocos);
        }
    }

}
