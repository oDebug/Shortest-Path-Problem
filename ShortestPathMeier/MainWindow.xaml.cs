using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ShortestPathMeier
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        Node[] nodes;
        int shortestDist = 10000000;
        //int startingIndex = -1;

        public MainWindow()
        {
            InitializeComponent();
        }

        public void Next(string history, int distance, string destination1, Node node1)
        {           
            
            //Check to see if recursion arrived at destination
            if (node1.city == destination1)
            {
                
                if (shortestDist > distance) //If this recursion leaf is less than the current least, replace it
                {
                    shortestDist = distance;
                }                

            }
            else if (!history.Contains(node1.city))
            {                
                history += node1.city;

                for (int x = 0; x < node1.connectedNodes.Length; x++) //Using recursion, go to every connecting node
                {
                    int newDistance = distance + node1.edges[x];
                    Next(history, newDistance, destination1, nodes[node1.connectedNodes[x]]);
                }
            }
        }

        private void btnDistance_Click(object sender, RoutedEventArgs e) //gets distance
        {
            string tempDestination = "|" + cboxTo.Text + "|"; //destination
            string tempStartingPoint = "|" + cboxFrom.Text + "|"; //starting point
            
            int i = 0;
            int j = 0;
            //int startingDist = -1;
            /////////GET INITIAL DISTANCE
            for (int x = 0; x < nodes.Length; x++)
            {
                if (nodes[x].city == tempDestination)
                {
                    i = x; //gets index of destination                    
                }
                if (nodes[x].city == tempStartingPoint)
                {
                    j = x; //Gets index of starting point
                }
            }            

            Next("", 0, tempDestination, nodes[j]);
            tboxDistance.Text = shortestDist.ToString();
            shortestDist = 100000000;
        }

        private void btnAddCityName_Click(object sender, RoutedEventArgs e)
        {
            Node tempNode = new Node(tboxAddCityName.Text); //Create a new node with this city name
            AddNode(tempNode); //Add that node to the "nodes" Array

            cboxAddCity.Items.Add(tboxAddCityName.Text);
            cboxFrom.Items.Add(tboxAddCityName.Text);
            cboxTo.Items.Add(tboxAddCityName.Text);

            tboxAddCityName.Text = ""; //Clear tbox
        }

        private void btnCreateLink_Click(object sender, RoutedEventArgs e)
        {
            string otherCity = "|" + cboxAddCity.Text + "|"; //Get the City2 that we are connecting
            string currentCity = "|" + tboxAddCityName.Text + "|"; //Get the City1 that we are connecting
            int distanceAmount = Convert.ToInt32(tboxAddDistance.Text);
            int indexOfOtherCity = 0,
                indexOfCurrentCity = 0;
            bool[] found = { false, false };

            for (int x = 0; x < nodes.Length; x++)
            {
                if (nodes[x].city == otherCity)
                {
                    indexOfOtherCity = x;
                    found[1] = true;
                }
                if (nodes[x].city == currentCity)
                {
                    indexOfCurrentCity = x;
                    found[0] = true;
                }
            }

            if (found[0] && found[1])
            {
                nodes[indexOfCurrentCity].AddNode(indexOfOtherCity, distanceAmount);
                nodes[indexOfOtherCity].AddNode(indexOfCurrentCity, distanceAmount);                
            }
        }

        public void AddNode(Node nodeToAdd)
        {
            Node[] tempNodeArray = { nodeToAdd };
            if (nodes != null)
            {
                nodes = nodes.Concat(tempNodeArray).ToArray();
            }
            else
            {
                nodes = tempNodeArray;
            }
        }

    }

    public class Node
    {

        public int[] connectedNodes;
        public int[] edges;
        public string city;

        public Node(string city1)
        {
            city = "|" + city1 + "|";
        }

        public void AddNode(int indexOfAddedNode, int edge)
        {
            int[] temp = {indexOfAddedNode};
            int[] temp2 = { edge };
            if (connectedNodes != null)
            {
                connectedNodes = connectedNodes.Concat(temp).ToArray();
            }
            else
            {
                connectedNodes = temp;
            }

            temp2[0] = edge;
            if (edges != null)
            {
                edges = edges.Concat(temp2).ToArray();
            }
            else
            {
                edges = temp2;
            }
        }

    }
}
