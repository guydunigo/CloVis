namespace ResumeStructure
{
    public abstract class Box
    {
        public Box(double x = 0, double y = 0, double z = 0, double sizeX = 10, double sizeY = 10, double angle = 0)
        {
            X = x;
            Y = y;
            Z = z;
            SizeX = sizeX;
            SizeY = sizeY;
            Angle = angle;
        }

        /// <summary>
        /// Millimeters from the top-left corner
        /// </summary>
        public double X { get; set; }
        /// <summary>
        /// Millimeters from the top-left corner
        /// </summary>
        public double Y { get; set; }
        protected double z;
        /// <summary>
        /// Box depth position, determines whether an element is above another one or below.
        /// </summary>
        public abstract double Z { get; set; }

        protected double sizeX;
        /// <summary>
        /// Millimeters
        /// </summary>
        public double SizeX { get => sizeX; set => sizeX = value > 0 ? value : 0; }
        protected double sizeY;
        /// <summary>
        /// Millimeters
        /// </summary>
        public double SizeY { get => sizeY; set => sizeY = value > 0 ? value : 0; }

        protected double angle;
        /// <summary>
        /// Defines the angle of the box in degrees clockwise
        /// </summary>
        public double Angle { get => angle; set => angle = value % 360; }

        /// <summary>
        /// "Deep" copy of the box
        /// </summary>
        /// <returns></returns>
        public abstract Box Copy();
    }
}
