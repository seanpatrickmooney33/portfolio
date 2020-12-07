
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.Routing;
using System.ComponentModel.DataAnnotations;
using NUnit.Framework;

namespace Prototype.Test.Utility
{
    public static class TestUtility
    {

        public static T InitPageModel<T>(Func<T> ctor) where T : PageModel
        {
            var services = new ServiceCollection();
            services.AddSingleton<IModelBinderFactory>(Mock.Of<IModelBinderFactory>());
            services.AddSingleton<IObjectModelValidator>(Mock.Of<IObjectModelValidator>());
            services.AddSingleton<IModelMetadataProvider>(Mock.Of<IModelMetadataProvider>());

            var httpContext = new DefaultHttpContext()
            {
                RequestServices = services.BuildServiceProvider()
            };
            var modelState = new ModelStateDictionary();
            var actionContext = new ActionContext(httpContext, new RouteData(), new PageActionDescriptor(), modelState);
            IModelMetadataProvider modelMetadataProvider = new EmptyModelMetadataProvider();
            var viewData = new ViewDataDictionary(modelMetadataProvider, modelState);
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());
            var pageContext = new PageContext(actionContext) { ViewData = viewData };


            T result = ctor();

            result.PageContext = pageContext;
            result.TempData = tempData;
            result.Url = new UrlHelper(actionContext);
            result.MetadataProvider = modelMetadataProvider;

            return result;
        }


        public static void ApplyModelState(this PageModel controller, Object parameter) { controller.ApplyModelState(new Object[] { parameter }); }
        public static void ApplyModelState(this PageModel controller, Object[] parameter)
        {
            controller.ModelState.Clear();
            if (parameter != null)
            {
                foreach (object model in parameter)
                {
                    if (model != null)
                    {
                        var validationContext = new ValidationContext(model);
                        var validationResults = new List<ValidationResult>();
                        Validator.TryValidateObject(model, validationContext, validationResults, true);
                        foreach (var validationResult in validationResults)
                        {
                            controller.ModelState.AddModelError(validationResult.MemberNames.First(), validationResult.ErrorMessage);
                        }
                    }
                }
            }
        }

        public static async Task Call<T>(this PageModel pageModel, Func<Task<IActionResult>> predicate, Object param = null) where T : ActionResult
        {
            await pageModel.Call<T>(predicate, new Object[] { param });
        }

        public static async Task Call<T>(this PageModel pageModel, Func<Task<IActionResult>> predicate, Object[] param) where T : ActionResult
        {
            if (param != null)
            {
                pageModel.ApplyModelState(param);
            }

            IActionResult result = await predicate();
            Assert.IsInstanceOf<T>(result);
        }


    }
}
