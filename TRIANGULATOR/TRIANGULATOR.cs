﻿using System;
using FORBES.LOGGER_NAMESPACE;

namespace FORBES.TRIANGULATOR_NAMESPACE
{
    /// <summary>
    /// This data structure was kind of a last minute addition 2020-10-22. Its used in the TRIANGLUATOR.TRIAGULATE method to return all variants of a triangulated point.
    /// because real world measurements wont produce exactly identical points of intersection. This structure can help detect outliers or miscalculations.
    /// </summary>
    public struct INTERSECTIONS_OUT_STRUCT
    {
        /// <summary>
        /// The point of intersection as generated by point-to-datum-1 and point-to-datum-2.
        /// </summary>
        public POINT_STRUCT RESULT_GENERATED_BY_1_2;
        /// <summary>
        /// The point of intersection as generated by point-to-datum-1 and point-to-datum-3.
        /// </summary>
        public POINT_STRUCT RESULT_GENERATED_BY_1_3;
        /// <summary>
        /// The point of intersection as generated by point-to-datum-2 and point-to-datum-3.
        /// </summary>
        public POINT_STRUCT RESULT_GENERATED_BY_2_3;
    }

    /// <summary>
    /// Structure that holds a real world cartesean point in double format.
    /// </summary>
    public struct POINT_STRUCT
    {
        /// <summary>
        /// Structure that holds a real world cartesean point in double format.
        /// </summary>
        /// <param name="X">X coordinate.</param>
        /// <param name="Y">Y coordinate.</param>
        public POINT_STRUCT(double X, double Y)
        {
            this.X = X;
            this.Y = Y;
        }
        /// <summary>
        /// X coordinate.
        /// </summary>
        public double X;
        /// <summary>
        /// Y coordinate.
        /// </summary>
        public double Y;
    }

    /// <summary>
    /// This structure is how measurements need to be packaged for triangulation.
    /// </summary>
    public struct DATA_STRUCT
    {
        /// <summary>
        /// The point in cartesean space where datum 1 is.
        /// </summary>
        public POINT_STRUCT DATUM_1;
        /// <summary>
        /// The point in cartesean space where datum 2 is.
        /// </summary>
        public POINT_STRUCT DATUM_2;
        /// <summary>
        /// The point in cartesean space where datum 3 is.
        /// </summary>
        public POINT_STRUCT DATUM_3;
        /// <summary>
        /// The distance the current point is to datum 1.
        /// </summary>
        public Double DISTANCE_TO_DATUM_1;
        /// <summary>
        /// The distance the current point is to datum 2.
        /// </summary>
        public Double DISTANCE_TO_DATUM_2;
        /// <summary>
        /// The distance the current point is to datum 3.
        /// </summary>
        public Double DISTANCE_TO_DATUM_3;
    }

/// <summary>
/// This class can triangulate where a point is in space given three datums and the distances to those datums. This is useful for plotting perimeters
/// using stakes and surveyors measuring tape.
/// </summary>
   public class TRIANGULATOR
    {
        /// <summary>
        /// The public log for all events in the current TRIANGULATOR instance.
        /// </summary>
        public LOGGER EVENTS = new LOGGER("Triangulator Log");
        private struct CIRCLE_STRUCT
        {
            public CIRCLE_STRUCT(POINT_STRUCT CENTER, double RADIUS)
            {
                this.CENTER = CENTER;
                this.RADIUS = RADIUS;
            }
            public POINT_STRUCT CENTER;
            public double RADIUS;

        }
        private struct INTERSECTION_STRUCT
        {
            public POINT_STRUCT A_1_2;
            public POINT_STRUCT B_1_2;
            public POINT_STRUCT A_1_3;
            public POINT_STRUCT B_1_3;
            public POINT_STRUCT A_2_3;
            public POINT_STRUCT B_2_3;
            public POINT_STRUCT RESULT_GENERATED_BY_1_2;
            public POINT_STRUCT RESULT_GENERATED_BY_1_3;
            public POINT_STRUCT RESULT_GENERATED_BY_2_3;
        }
        
        /// <summary>
        /// Constructor, nothing special.
        /// </summary>
        public TRIANGULATOR()
        {
            EVENTS.LOG_MESSAGE(1, "INITIALIZE");
        }

        //Methods
        /// <summary>
        /// This method will output where a given point is in space.
        /// </summary>
        /// <param name="INPUT_DATA">The structure of all the measurements. Refer to structure documentation for construction.</param>
        /// <param name="LOCATION">OUT parameter, this single point structure is the averaged point in space.</param>
        /// <param name="INTERSECTIONS_LIST">OUT parameter, this structure contains all the different points used to calculate the average point.</param>
        /// <returns>0 on success, 1 on fail.</returns>
        public int TRIANGULATE(DATA_STRUCT INPUT_DATA, out POINT_STRUCT LOCATION, out INTERSECTIONS_OUT_STRUCT INTERSECTIONS_LIST)
        {
            EVENTS.LOG_MESSAGE(1, "ENTER");
            LOCATION = new POINT_STRUCT(); //Create a blank point structure to pass up to the calling function.
            INTERSECTIONS_LIST = new INTERSECTIONS_OUT_STRUCT();
            //Construct a circle structure based off the input data. Its just easier to reformat the data a bit
            //so that its labelled in circular terms rather than distances to datums. Bit easier to understand.
            CIRCLE_STRUCT CIRCLE_1 = new CIRCLE_STRUCT(INPUT_DATA.DATUM_1, INPUT_DATA.DISTANCE_TO_DATUM_1);
            CIRCLE_STRUCT CIRCLE_2 = new CIRCLE_STRUCT(INPUT_DATA.DATUM_2, INPUT_DATA.DISTANCE_TO_DATUM_2);
            CIRCLE_STRUCT CIRCLE_3 = new CIRCLE_STRUCT(INPUT_DATA.DATUM_3, INPUT_DATA.DISTANCE_TO_DATUM_3);
            //Create a structure of all the points of intersection. It was a bit easier on organization to have
            //a structure containing all the points, rather than independently declaring everything.
            INTERSECTION_STRUCT INTERSECTION = new INTERSECTION_STRUCT();
            //Determine the points of intersection between circles.
            int SUMMED_RETURN = 0; //Holds the values returned from the functions below.
            EVENTS.LOG_MESSAGE(2, "Processing intersection of circles 1 & 2.");
            SUMMED_RETURN += FIND_INTERSECTIONS(CIRCLE_1, CIRCLE_2, out INTERSECTION.A_1_2, out INTERSECTION.B_1_2); //Go to a function to find the points of intesection between the two circles.
            EVENTS.LOG_MESSAGE(2, "Processing intersection of circles 1 & 3.");                                      //If its not an edge case, the function will return two points of intersection
            SUMMED_RETURN += FIND_INTERSECTIONS(CIRCLE_1, CIRCLE_3, out INTERSECTION.A_1_3, out INTERSECTION.B_1_3); //through the OUT arguments. If there is an edge case, like no intersection or
            EVENTS.LOG_MESSAGE(2, "Processing intersection of circles 2 & 3.");                                      //single point intersection, then the function will return a non-zero number
            SUMMED_RETURN += FIND_INTERSECTIONS(CIRCLE_2, CIRCLE_3, out INTERSECTION.A_2_3, out INTERSECTION.B_2_3); //and this function will return out below.
            if (SUMMED_RETURN != 0) //If any of the circle intersections are a special case...
            {
                EVENTS.LOG_MESSAGE(2, "EXCEPTION", "One of the FIND_INTERSECTIONS calls returned a edge case.");
                EVENTS.LOG_MESSAGE(1, "EXIT_FAIL");
                return 1; //exit this function in an error state.
            }
            //Find the smallest distance between the points of intersection in 1,2 and 1,3.
            //We need to do this to find out which points should be the same, seeing as there is rounding, floating point, and measurement error,
            //The points will never be EXACTLY the same, so we need to manually determine which values are representing the same nominal point.
            double DIST_1 = DETERMINE_DISTANCE(INTERSECTION.A_1_2, INTERSECTION.A_1_3); //Go to a function to calculate distance.
            double DIST_2 = DETERMINE_DISTANCE(INTERSECTION.A_1_2, INTERSECTION.B_1_3); //Go to a function to calculate distance.
            double DIST_3 = DETERMINE_DISTANCE(INTERSECTION.B_1_2, INTERSECTION.A_1_3); //Go to a function to calculate distance.
            double DIST_4 = DETERMINE_DISTANCE(INTERSECTION.B_1_2, INTERSECTION.B_1_3); //Go to a function to calculate distance.
            double SMALLEST_DIST; //Holds the smallest distance from evaluation set 1, to be used in evaluation set 2.
                                  //Evaluation set 1, Determine if DIST_1 or DIST_2 is smaller.
            if (DIST_1 < DIST_2) //If DIST_1 is smaller...
            {
                SMALLEST_DIST = DIST_1; //Record that distance.
                INTERSECTION.RESULT_GENERATED_BY_1_2 = INTERSECTION.A_1_2; //Store the point set.
                INTERSECTION.RESULT_GENERATED_BY_1_3 = INTERSECTION.A_1_3; //Store the point set.
            }
            else //If DIST_2 is smaller...
            {
                SMALLEST_DIST = DIST_2; //Record that distance.
                INTERSECTION.RESULT_GENERATED_BY_1_2 = INTERSECTION.A_1_2; //Store the point set.
                INTERSECTION.RESULT_GENERATED_BY_1_3 = INTERSECTION.B_1_3; //Store the point set.
            }
            //Evaluation set 2, Determine if DIST_3 is smaller than DIST_4 while also being smaller than the result from eval set 1.
            if (DIST_3 < DIST_4 && DIST_3 < SMALLEST_DIST) //If DIST_3 is less than DIST_4 and smaller than the result from eval set 1...
            {
                INTERSECTION.RESULT_GENERATED_BY_1_2 = INTERSECTION.B_1_2; //Store the point set.
                INTERSECTION.RESULT_GENERATED_BY_1_3 = INTERSECTION.A_1_3; //Store the point set.
            }
            else if (DIST_4 < DIST_3 && DIST_4 < SMALLEST_DIST) //Else, if DIST_4 is smaller while still being smaller than the result from eval set 1...
            {
                INTERSECTION.RESULT_GENERATED_BY_1_2 = INTERSECTION.B_1_2; //Store the point set.
                INTERSECTION.RESULT_GENERATED_BY_1_3 = INTERSECTION.B_1_3; //Store the point set.
            }
            //At this point, all the points of intersection have been calculated and we have determined the distance between all the points
            //of circle set 1,2 and circle set 1,3. INTERSECTION.RESULT_GENERATED_BY_1_2 will hold the intersection point of the 3 circles as generated by circle set 1,2.
            //INTERSECTION.RESULT_GENERATED_BY_1_3 will hold the intersection point of the 3 circles as generated by circle set 1,3. Now we know where the point is but we still
            //want to find the intersection point as generated by 2,3 so that we can average out the 3 points to create a more accurate single point to return.
            //We'll use INTERSECTION.RESULT_GENERATED_BY_1_2 and to compare the points in circle set 2,3 to quickly find which point from that circle set is representing the
            //3 point intersection.
            double DIST_5 = DETERMINE_DISTANCE(INTERSECTION.RESULT_GENERATED_BY_1_2, INTERSECTION.A_2_3);
            double DIST_6 = DETERMINE_DISTANCE(INTERSECTION.RESULT_GENERATED_BY_1_2, INTERSECTION.B_2_3);
            if (DIST_5 < DIST_6)
                INTERSECTION.RESULT_GENERATED_BY_2_3 = INTERSECTION.A_2_3;
            else
                INTERSECTION.RESULT_GENERATED_BY_2_3 = INTERSECTION.B_2_3;
            //Now we should have all three points that represent the point of intersection of the three circles. Now a simple average is all thats needed to return a final point.
            LOCATION.X = (INTERSECTION.RESULT_GENERATED_BY_1_2.X + INTERSECTION.RESULT_GENERATED_BY_1_3.X + INTERSECTION.RESULT_GENERATED_BY_2_3.X) / 3;
            LOCATION.Y = (INTERSECTION.RESULT_GENERATED_BY_1_2.Y + INTERSECTION.RESULT_GENERATED_BY_1_3.Y + INTERSECTION.RESULT_GENERATED_BY_2_3.Y) / 3;
            //ALL DONE!
            INTERSECTIONS_LIST.RESULT_GENERATED_BY_1_2 = INTERSECTION.RESULT_GENERATED_BY_1_2; //REV 2020-10-22.
            INTERSECTIONS_LIST.RESULT_GENERATED_BY_1_3 = INTERSECTION.RESULT_GENERATED_BY_1_3; //REV 2020-10-22.
            INTERSECTIONS_LIST.RESULT_GENERATED_BY_2_3 = INTERSECTION.RESULT_GENERATED_BY_2_3; //REV 2020-10-22.
            EVENTS.LOG_MESSAGE(2, String.Format("Intersection point: {0}, {1}", LOCATION.X.ToString("00.000"), LOCATION.Y.ToString("00.000")));
            EVENTS.LOG_MESSAGE(1, "EXIT_SUCCESS");
            return 0;
        } //Main public method.
        private double DETERMINE_DISTANCE(POINT_STRUCT A, POINT_STRUCT B)
        {
            double DIST_X = A.X - B.X;
            double DIST_Y = A.Y - B.Y;
            return Math.Sqrt(DIST_X * DIST_X + DIST_Y * DIST_Y);
        } //Calculates the distance between two points.
        private int FIND_INTERSECTIONS(CIRCLE_STRUCT CIRCLE_A, CIRCLE_STRUCT CIRCLE_B, out POINT_STRUCT INTERSECTION_A, out POINT_STRUCT INTERSECTION_B)
        {
            EVENTS.LOG_MESSAGE(1, "ENTER");
            //Set the outputs in case of a premature return.
            INTERSECTION_A = new POINT_STRUCT();
            INTERSECTION_B = new POINT_STRUCT();

            //Find the distance between centers.
            double DIST = DETERMINE_DISTANCE(CIRCLE_A.CENTER, CIRCLE_B.CENTER);
            EVENTS.LOG_MESSAGE(2, string.Format("Distance between centers: {0}", DIST.ToString("00.000")));

            //Check for edge cases.
            if (DIST > CIRCLE_A.RADIUS + CIRCLE_B.RADIUS) //Circles are too far apart.
            {
                EVENTS.LOG_MESSAGE(2, "EXCEPTION", "Circles are too far apart.");
                EVENTS.LOG_MESSAGE(1, "EXIT_FAIL");
                return 1;
            }
            if (DIST < Math.Abs(CIRCLE_A.RADIUS - CIRCLE_B.RADIUS)) //One circle is within the other.
            {
                EVENTS.LOG_MESSAGE(2, "EXCEPTION", "One circle is within the other.");
                EVENTS.LOG_MESSAGE(1, "EXIT_FAIL");
                return 2;
            }
            if ((DIST == 0) && (CIRCLE_A.RADIUS == CIRCLE_B.RADIUS)) //The cirles are the same.
            {
                EVENTS.LOG_MESSAGE(2, "EXCEPTION", "The cirles are the same.");
                EVENTS.LOG_MESSAGE(1, "EXIT_FAIL");
                return 3;
            }
            if (DIST == CIRCLE_A.RADIUS + CIRCLE_B.RADIUS)  //The circles only intersect at one point.
            {
                EVENTS.LOG_MESSAGE(2, "EXCEPTION", "The circles only intersect at one point.");
                EVENTS.LOG_MESSAGE(1, "EXIT_FAIL");
                return 3;
            }

            //Find A and H.
            double A = (CIRCLE_A.RADIUS * CIRCLE_A.RADIUS - CIRCLE_B.RADIUS * CIRCLE_B.RADIUS + DIST * DIST) / (2 * DIST);
            double H = Math.Sqrt(CIRCLE_A.RADIUS * CIRCLE_A.RADIUS - A * A);

            //Find intermediate values.
            double INTERMEDIATE_X = CIRCLE_A.CENTER.X + A * (CIRCLE_B.CENTER.X - CIRCLE_A.CENTER.X) / DIST;
            double INTERMEDIATE_Y = CIRCLE_A.CENTER.Y + A * (CIRCLE_B.CENTER.Y - CIRCLE_A.CENTER.Y) / DIST;

            //Perform final math for intersections.
            INTERSECTION_A.X = INTERMEDIATE_X + H * (CIRCLE_B.CENTER.Y - CIRCLE_A.CENTER.Y) / DIST;
            INTERSECTION_A.Y = INTERMEDIATE_Y - H * (CIRCLE_B.CENTER.X - CIRCLE_A.CENTER.X) / DIST;
            INTERSECTION_B.X = INTERMEDIATE_X - H * (CIRCLE_B.CENTER.Y - CIRCLE_A.CENTER.Y) / DIST;
            INTERSECTION_B.Y = INTERMEDIATE_Y + H * (CIRCLE_B.CENTER.X - CIRCLE_A.CENTER.X) / DIST;
            EVENTS.LOG_MESSAGE(2, string.Format("Intersection A detected at: {0}, {1}", INTERSECTION_A.X.ToString("00.000"), INTERSECTION_A.Y.ToString("00.000")));
            EVENTS.LOG_MESSAGE(2, string.Format("Intersection B detected at: {0}, {1}", INTERSECTION_B.X.ToString("00.000"), INTERSECTION_B.Y.ToString("00.000")));
            EVENTS.LOG_MESSAGE(1, "EXIT_SUCCESS");
            return 0;

        } //Finds where two circles intersect.

    }
}