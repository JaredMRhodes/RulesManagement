using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;
using RulesManagement.Sets;

namespace RulesManagement.Mvc
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
    public sealed class RulesAttribute : ActionFilterAttribute
    {
        private IValidationSet _validationSet;

        public  Type ValidationSetType {get; set;}

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (_validationSet == null)
            {
                this.CreateValidationSet();
            }
            using (this._validationSet)
            {
                this._validationSet.RunValidaitonRules(filterContext.ActionParameters.Values);
                foreach (var message in this._validationSet.ValidationResult.GetMessageOfType(x => x != Validation.ValidationState.Passed))
                {
                    filterContext.Controller.ViewData.ModelState.AddModelError("ValidaitonSetError", message.Message);
                }
            }
            base.OnActionExecuting(filterContext);
        }

        private void CreateValidationSet()
        {
            var constructor = ValidationSetType.GetConstructor(Type.EmptyTypes);
            if (constructor == null)
            {
                throw new ArgumentException("No Empty constructor for Type: " + ValidationSetType.FullName + ". Must provide Validaiton Set with Empty Constructor");
            }
            if (!typeof(IValidationSet).IsAssignableFrom(ValidationSetType))
            {
                throw new ArgumentException("The type provided must be assignable to type IValidationSet");
            }
            this._validationSet = (IValidationSet)System.Activator.CreateInstance(ValidationSetType);
        }
    }
}
