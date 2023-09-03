using System.Text.Json;
using System.Text.Json.Serialization;
using Diablo4.Shared.Extensions;
using Diablo4.Shared.Types;

namespace Diablo4.Shared.Json.Converters;

public class WorldBossJsonConverter : JsonConverter<WorldBoss>
{
    /// <inheritdoc />
    public override WorldBoss Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) =>
        throw new NotImplementedException();

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, WorldBoss value, JsonSerializerOptions options) =>
        writer.WriteStringValue(value.ToDescription());
}
