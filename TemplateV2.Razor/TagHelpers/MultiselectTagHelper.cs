using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TemplateV2.Models.DomainModels;

namespace TemplateV2.Razor.TagHelpers
{
    [HtmlTargetElement("multiselect", Attributes = "asp-items, asp-for")]
    public class MultiSelectTagHelper : TagHelper
    {
        private string ElementId { get { return SelectedValues?.Name.Replace(".", "_") ?? string.Empty; } } // tag names are generated with a dot (.) which makes it difficulte for jquery selections

        private string ElementName { get { return SelectedValues?.Name ?? string.Empty; } }

        /// <summary>
        /// Gets or sets the items that are bound to this multiselect list
        /// </summary>
        [HtmlAttributeName("asp-items")]
        public IEnumerable<SelectListItem>? Items { get; set; }

        /// <summary>
        /// Gets or sets the selected values for the list
        /// </summary>
        [HtmlAttributeName("asp-for")]
        public ModelExpression? SelectedValues { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "select";
            output.TagMode = TagMode.StartTagAndEndTag;

            output.Attributes.SetAttribute("id", ElementId);
            output.Attributes.SetAttribute("class", "selectpicker form-control");
            output.Attributes.SetAttribute(new TagHelperAttribute("multiple"));

            if (SelectedValues?.Model != null && Items != null)
            {
                var sb = new StringBuilder();

                bool mustGroup = Items.Any(i => i.Group != null);
                if (mustGroup)
                {
                    foreach (var groupedItems in Items.GroupBy(i => i.Group.Name))
                    {
                        sb.AppendLine($"<optgroup label='{groupedItems.Key}'>");
                        foreach (var item in groupedItems)
                        {
                            var disabledAttribute = item.Disabled ? "disabled" : string.Empty;
                            var selectedAttribute = ((List<int>)SelectedValues.Model).Any(c => c == int.Parse(item.Value)) ? "selected" : string.Empty;
                            sb.AppendLine($"<option value='{item.Value}' {selectedAttribute} {disabledAttribute}>{item.Text}</option>");
                        }
                        sb.AppendLine($"</optgroup>");
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

                // configure selected inputs container
                sb = new StringBuilder();
                sb.AppendLine($"<div class='selectpicker-data'>");
                foreach (var value in SelectedValues.Model as List<int> ?? new List<int>())
                {
                    sb.AppendLine($"<input type='hidden' name='{ElementName}' value='{value}' />");
                }
                sb.AppendLine($"</div>");
                output.PostElement.AppendHtml(sb.ToString());
            }
        }
    }

    public static class SelectListExtensions
    {
        public static List<SelectListItem> ToSelectList(this List<PermissionEntity> items)
        {
            return items.Select(i =>
            new SelectListItem()
            {
                Text = i.Name,
                Value = i.Id.ToString(),
                Group = new SelectListGroup()
                {
                    Name = i.Group_Name
                }
            }).ToList();
        }

        public static List<SelectListItem> ToSelectList(this List<RoleEntity> items)
        {
            return items.Select(i =>
            new SelectListItem()
            {
                Text = i.Name,
                Value = i.Id.ToString()
            }).ToList();
        }

    }
}
