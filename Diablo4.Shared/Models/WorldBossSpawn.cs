using System.Text.Json.Serialization;
using Diablo4.Shared.Types;

namespace Diablo4.Shared.Models;

public record WorldBossSpawn(WorldBoss Boss, DateTime Time);
