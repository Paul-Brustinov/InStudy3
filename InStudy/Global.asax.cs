using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using FluentValidation.Mvc;

namespace InStudy
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

           FluentValidationModelValidatorProvider.Configure();

            // Remove data annotations validation provider 
            //ModelValidatorProviders.Providers.Remove(
            //            ModelValidatorProviders.Providers.OfType<DataAnnotationsModelValidatorProvider>().First());
        }
}
}
