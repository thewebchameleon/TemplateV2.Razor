using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TemplateV2.Models;
using TemplateV2.Models.DomainModels;

namespace TemplateV2.Razor.TagHelpers
{
    [HtmlTargetElement("multicheckbox", Attributes = "asp-items, asp-for")]
    public class MultiCheckboxTagHelper : TagHelper
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        private string ElementId { get { return SelectedValues.Name.Replace(".", "_"); } } // tag names are generated with a dot (.) which makes it difficulte for jquery selections

        private string ElementName { get { return SelectedValues.Name; } }

        public MultiCheckboxTagHelper(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Gets or sets the items that are bound to this multiselect list
        /// </summary>
        [HtmlAttributeName("asp-items")]
        public IEnumerable<CheckboxListItem> Items { get; set; }

        /// <summary>
        /// Gets or sets the selected values for the list
        /// </summary>
        [HtmlAttributeName("asp-for")]
        public ModelExpression SelectedValues { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            output.TagMode = TagMode.StartTagAndEndTag;

            //output.Attributes.SetAttribute("class", "selectpicker form-control");
            //output.Attributes.SetAttribute(new TagHelperAttribute("multiple"));

            if (SelectedValues.Model != null)
            {
                var sb = new StringBuilder();

                bool mustGroup = Items.Any(i => i.Group != null);
                if (mustGroup)
                {
                    int index = 0;
                    foreach (var groupedItems in Items.GroupBy(i => i.Group.Name))
                    {
                        sb.AppendLine($"<fieldset>");
                        sb.AppendLine($"<legend class=''>{groupedItems.Key}</legend>");
                        sb.AppendLine($"<div class='form-row mx-0'>");
                        foreach (var item in groupedItems)
                        {
                            var disabledAttribute = item.Disabled ? "disabled" : string.Empty;
                            var selectedAttribute = ((List<CheckboxItemSelection>)SelectedValues.Model).Any(c => c.Id == int.Parse(item.Value)) ? "checked" : string.Empty;

                            sb.AppendLine($"<div class='form-group d-flex align-items-center mr-3'>");
                            sb.AppendLine($"<input type='hidden' name='{ElementName}[{index}].Id' value='{item.Value}'>");
                            sb.AppendLine($"<input type='checkbox' id='{ElementId}[{index}].Id' name='{ElementName}[{index}].Selected' value='true' {disabledAttribute} {selectedAttribute} />");
                            sb.AppendLine($"<label for='{ElementId}[{index}].Id'></label>");
                            sb.AppendLine($"<label for='{ElementId}[{index}].Id'>{item.Text}</label>");
                            sb.AppendLine($"</div>");
                            index++;
                        }
                        sb.AppendLine($"</div>");
                        sb.AppendLine($"</fieldset>");
                    }
                }
                else
                {
                    foreach (var item in Items)
                    {
                        var disabledAttribute = item.Disabled ? "disabled" : string.Empty;
                        var selectedAttribute = ((List<int>)SelectedValues.Model).Any(c => c == int.Parse(item.Value)) ? "selected" : string.Empty;
                        sb.AppendLine($"<option value='{item.Value}' {selectedAttribute} {disabledAttribute}>{item.Text}</option>");
                    }
                }
                output.PreContent.SetHtmlContent(sb.ToString());
            }
        }
    }

    public static class CheckboxListExtensions
    {
        public static List<CheckboxListItem> ToCheckboxList(this List<PermissionEntity> items)
        {
            return items.Select(i =>
            new CheckboxListItem()
            {
                Text = i.Name,
                Value = i.Id.ToString(),
                Group = new SelectListGroup()
                {
                    Name = i.Group_Name
                }
            }).ToList();
        }
    }

    public class CheckboxListItem
    {
        public string Text { get; set; }

        public string Value { get; set; }

        public SelectListGroup Group { get; set; }

        public bool Disabled { get; set; }

        public bool Selected { get; set; }
    }
}
