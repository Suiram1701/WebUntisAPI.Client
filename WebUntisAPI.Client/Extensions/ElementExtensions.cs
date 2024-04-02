using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebUntisAPI.Client.Models.Elements;
using WebUntisAPI.Client.Models.Interfaces;

namespace WebUntisAPI.Client.Extensions;

/// <summary>
/// Build-in extensions for <see cref="IElement"/>
/// </summary>
public static class ElementExtensions
{
    /// <summary>
    /// Determines the <see cref="ElementType"/> that is assigned to this instance
    /// </summary>
    /// <returns>The type</returns>
    public static ElementType GetElementType(this IElement element)
    {
        return element.GetType().Name switch
        {
            nameof(Class) => ElementType.Class,
            nameof(Teacher) => ElementType.Teacher,
            nameof(Subject) => ElementType.Subject,
            nameof(Room) => ElementType.Room,
            nameof(Student) => ElementType.Student,
            _ => throw new NotImplementedException($"The element typ {element.GetType().Name} isn't implemented.")
        };
    }
}
