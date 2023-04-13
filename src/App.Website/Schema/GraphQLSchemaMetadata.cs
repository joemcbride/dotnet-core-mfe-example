namespace App.Website.Schema;

public class GraphQLSchemaMetadataAttribute : Attribute
{
    public GraphQLSchemaMetadataAttribute(string schema)
    {
        Schema = schema;
    }

    public string Schema { get; set; }
}
