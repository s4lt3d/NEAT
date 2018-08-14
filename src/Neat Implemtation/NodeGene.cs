namespace Neat_Implemtation {
    public class NodeGene
    {
        public enum NodeType {
            INPUT_NODE, 
            OUTPUT_NODE,
            HIDDEN_NODE,
            BIAS_NODE
        }

        NodeType type;
        int id;

        public NodeType Type { get => type;  }
        public int Id { get => id; }

        public NodeGene(NodeType type, int id) {
            this.type = type;
            this.id = id;
        }

        public NodeGene replicate() {
            return new NodeGene(type, id);
        }

        public override string ToString() {
            string s = "{";
            if (type == NodeType.INPUT_NODE)
                s += "INPUT,  ";
            else if (type == NodeType.OUTPUT_NODE)
                s += "OUTPUT, ";
            else if (type == NodeType.HIDDEN_NODE)
                s += "HIDDEN, ";
            else if (type == NodeType.BIAS_NODE)
                s += "BIAS,   ";

            s += id + "}";
            return s;
        }
    }
}
