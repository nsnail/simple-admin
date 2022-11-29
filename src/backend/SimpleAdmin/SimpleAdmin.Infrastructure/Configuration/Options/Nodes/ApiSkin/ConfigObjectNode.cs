using System.Text.Json.Serialization;

namespace SimpleAdmin.Infrastructure.Configuration.Options.Nodes.ApiSkin;

/// <summary>
///     swagger 配置对象 节点
/// </summary>
public record ConfigObjectNode
{
    /// <summary>
    ///     附加item
    /// </summary>
    [JsonExtensionData]
    public Dictionary<string, object> AdditionalItems { get; set; } = new();

    /// <summary>
    ///     If set to true, enables deep linking for tags and operations
    /// </summary>
    public bool DeepLinking { get; set; } = false;

    /// <summary>
    ///     The default expansion depth for the model on the model-example section
    /// </summary>
    public int DefaultModelExpandDepth { get; set; } = 1;

    /// <summary>
    ///     Controls how the model is shown when the API is first rendered.
    ///     (The user can always switch the rendering for a given model by clicking the 'Model' and 'Example Value' links)
    /// </summary>
    public ModelRenderingNode DefaultModelRendering { get; set; } = ModelRenderingNode.Example;

    /// <summary>
    ///     The default expansion depth for models (set to -1 completely hide the models)
    /// </summary>
    public int DefaultModelsExpandDepth { get; set; } = 1;

    /// <summary>
    ///     Controls the display of operationId in operations list
    /// </summary>
    public bool DisplayOperationId { get; set; } = false;

    /// <summary>
    ///     Controls the display of the request duration (in milliseconds) for Try-It-Out requests
    /// </summary>
    public bool DisplayRequestDuration { get; set; } = false;

    /// <summary>
    ///     Controls the default expansion setting for the operations and tags.
    ///     It can be 'list' (expands only the tags), 'full' (expands the tags and operations) or 'none' (expands nothing)
    /// </summary>
    public DocExpansionNode DocExpansion { get; set; } = DocExpansionNode.List;

    /// <summary>
    ///     If set, enables filtering. The top bar will show an edit box that you can use to filter the tagged operations
    ///     that are shown. Can be an empty string or specific value, in which case filtering will be enabled using that
    ///     value as the filter expression. Filtering is case sensitive matching the filter expression anywhere inside the tag
    /// </summary>
    public string Filter { get; set; } = null;

    /// <summary>
    ///     If set, limits the number of tagged operations displayed to at most this many. The default is to show all
    ///     operations
    /// </summary>
    public int? MaxDisplayedTags { get; set; } = null;

    /// <summary>
    ///     OAuth redirect URL
    /// </summary>
    [JsonPropertyName("oauth2RedirectUrl")]
    public string OAuth2RedirectUrl { get; set; } = null;

    /// <summary>
    ///     If set to true, it persists authorization data and it would not be lost on browser close/refresh
    /// </summary>
    public bool PersistAuthorization { get; set; } = false;

    /// <summary>
    ///     Controls the display of extensions (pattern, maxLength, minLength, maximum, minimum) fields and values for
    ///     Parameters
    /// </summary>
    public bool ShowCommonExtensions { get; set; } = false;

    /// <summary>
    ///     Controls the display of vendor extension (x-) fields and values for Operations, Parameters, and Schema
    /// </summary>
    public bool ShowExtensions { get; set; } = false;

    /// <summary>
    ///     List of HTTP methods that have the Try it out feature enabled.
    ///     An empty array disables Try it out for all operations. This does not filter the operations from the display
    /// </summary>
    public IEnumerable<SubmitMethodNode> SupportedSubmitMethods { get; set; } =
        Enum.GetValues(typeof(SubmitMethodNode)).Cast<SubmitMethodNode>();

    /// <summary>
    ///     Controls whether the "Try it out" section should be enabled by default.
    /// </summary>
    [JsonPropertyName("tryItOutEnabled")]
    public bool TryItOutEnabled { get; set; }

    /// <summary>
    ///     One or more Swagger JSON endpoints (url and name) to power the UI
    /// </summary>
    public IEnumerable<UrlDescriptorNode> Urls { get; set; } = null;

    /// <summary>
    ///     By default, Swagger-UI attempts to validate specs against swagger.io's online validator.
    ///     You can use this parameter to set a different validator URL, for example for locally deployed validators (Validator
    ///     Badge).
    ///     Setting it to null will disable validation
    /// </summary>
    public string ValidatorUrl { get; set; } = null;
}