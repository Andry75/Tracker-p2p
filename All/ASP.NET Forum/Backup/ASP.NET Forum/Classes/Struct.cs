namespace ASP.NET_Forum
{

    public struct ForumItem
    {
        public int id;
        public string name;
        public string description;
    }

    public struct TopicItem
    {
        public int id;
        public string name;
        public int mess;
    }
    public struct MessageItem
    {
        public int id;
        public int user;
        public string text;
        
    }
}