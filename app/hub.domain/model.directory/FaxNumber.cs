namespace hub.domain.model.directory
{
    public class FaxNumber
    {
        public int FaxId { get; set; }
        public string FaxName { get; set; }
        public string Number { get; set; }

        public int? LocationId { get; set; }
        public Location Location { get; set; }

        public int? DepartmentId { get; set; }
        public Department Department { get; set; }
    }
}
