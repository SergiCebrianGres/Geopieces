public enum Figures2D {
    Square,
    Rectangle,
    Rhombus,
    Trapezium,
    EquilateralTriangle,
    IsoscelesTriangle,
    RectangleTriangle,
    ScaleneTriangle,
    RegularPentagon,
    IrregularPentagon,
    Circle,
    Cross,
    Heart,
    Spiral,
    Star,
    teacup,
    None
    };

public enum Figures3D { Cub, PiramideQuadrada, Tetraedre};

public enum NumOfEdges
{
    Three,
    Four,
    Five,
    Six,
    Seven,
    Eight,
    Nine,
    Ten,
    Eleven,
    Twelve,
    NoInt
};

public enum ObjectiveID
{
    findTriangles,
    findEquilateralTriangle,
    findIsoscelesTriangle,
    findRectangleTriangle,
    findScaleneTriangle,
    findSquares,
    findRectangles,
    findRhombus,
    findTrapeziums,
    findParallelogrms,
    findQuadrilaters,
    findPentagon,
    findRegularPentagon,
    findIrregularPentagon,
    findRegularPolygon,
    findNoIntEdges,
    findSymmetry,
    findReflectionalSymmetry,
    findRotationalSymmetry,
    findNoSymmetry,
    findFiguresWithRightAngle,
    findFiguresWithAcuteAngle,
    findFiguresWithObtuseAngle,
    findParallelograms,
    findFiguresWithTwoParalelEdges,
    findFiguresWithNoParalelEdges,
    findFigureWithMaxArea,
    findFigureWithMinArea,
    findFigureWithMaxPerimeter,
    findFigureWithMinPerimeter
};

public enum RewardType { Square, Triangle, Pentagon, TriangularPrism, PentagonalPrism, Cube, Pyramide, Octahedron, Tetrahedron, Coin, NoRewardType };

public enum InventoryItemType { type2D, type3D, coin };

public enum Competencies { Identification, Symmetry, Angles, AreaPerimeter };

public enum Size { VerySmall, Small, Medium, Big, VeryBig };

public enum Level { VeryEasy, Easy, Medium, Difficult, VeryDifficult };

public enum State2D { Correct, IncorrectNoTime, IncorrectWithTime }

