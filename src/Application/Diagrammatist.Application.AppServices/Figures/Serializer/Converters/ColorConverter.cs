using Nerdbank.MessagePack;
using PolyType.Abstractions;
using System.Drawing;
using System.Text.Json.Nodes;

namespace Diagrammatist.Application.AppServices.Figures.Serializer.Converters
{
    /// <summary>
    /// A stub class for color conversion until <see cref=“Nerdbank.MessagePack”/> is updated to a color-conversion-enabled version.
    /// </summary>
    public class ColorConverter : MessagePackConverter<Color>
    {
        /// <inheritdoc/>
        public override JsonObject? GetJsonSchema(JsonSchemaContext context, ITypeShape typeShape)
        {
            return new()
            {
                ["type"] = "integer",
                ["format"] = "int32",
                ["description"] = "An ARGB color value.",
            };
        }

        /// <inheritdoc/>
        public override Color Read(ref MessagePackReader reader, SerializationContext context)
        {
            return Color.FromArgb(reader.ReadInt32());
        }

        /// <inheritdoc/>
        public override void Write(ref MessagePackWriter writer, in Color value, SerializationContext context)
        {
            writer.Write(value.ToArgb());
        }
    }
}
