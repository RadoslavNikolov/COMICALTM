namespace Contests.App.Models.ViewModels
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using AutoMapper;
    using BindingModels;
    using Contests.Models;
    using Helpers;
    using Infrastructure.Mapping;

    public class UserEditViewModel : UserEditBindingModel, IMapFrom<User>
    {

    }
}