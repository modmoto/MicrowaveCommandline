//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:6.0.8
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Domain.Posts
{
    using System;
    using System.Collections.Generic;
    
    
    public class PostUpdateTitleCommand
    {
        
        public String Title { get; private set; }
        
        public PostUpdateTitleCommand(String Title)
        {
            this.Title = Title;
        }
    }
}
