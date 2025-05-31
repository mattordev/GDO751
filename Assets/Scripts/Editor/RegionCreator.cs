using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

public class RegionCreator : EditorWindow
{
    private List<Region> Regions = new List<Region>();
    private Vector2 scrollPosition; // For scrolling support

    [MenuItem("Tools/Region Creator")]
    public static void ShowWindow()
    {
        GetWindow<RegionCreator>("Region Creator");
    }

    public void OnGUI()
    {
        GUILayout.Label("Welcome to the Region Creator.");
        GUILayout.Space(15);
        GUILayout.Label("Tools:");
        GUILayout.Space(2.5f);

        if (GUILayout.Button("Find Regions"))
        {
            FindRegions();
        }
        if (GUILayout.Button("Clear Regions"))
        {
            Regions.Clear();
            Repaint();
        }

        if (GUILayout.Button("Create Regions"))
        {
            // CreateRegion();
        }

        if (Regions.Count > 0)
        {
            GUILayout.Space(10);
            GUILayout.Label("Found Regions:", EditorStyles.boldLabel);

            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, GUILayout.Height(200));

            int removeIndex = -1;

            for (int i = 0; i < Regions.Count; i++)
            {
                if (Regions[i] != null)
                {
                    EditorGUILayout.BeginHorizontal();

                    // Display region label with flexible spacing
                    GUILayout.Label($"Region {i + 1} ({Regions[i].regionName})", GUILayout.Width(185));

                    // Make the text field expand dynamically
                    string oldName = Regions[i].regionName;
                    string newName = EditorGUILayout.TextField(oldName, GUILayout.ExpandWidth(true));

                    // If the name changed, update the region and GameObject
                    if (newName != oldName)
                    {
                        Undo.RecordObject(Regions[i], "Change Region Name");
                        Regions[i].regionName = newName;
                        Regions[i].gameObject.name = $"Region ({newName})";
                        EditorUtility.SetDirty(Regions[i]);
                    }

                    if (GUILayout.Button("X", GUILayout.Width(20)))
                    {
                        removeIndex = i;
                    }

                    EditorGUILayout.EndHorizontal();
                }
            }

            EditorGUILayout.EndScrollView();

            if (removeIndex >= 0)
            {
                Regions.RemoveAt(removeIndex);
                Repaint();
            }
        }
    }

    private void FindRegions()
    {
        // Get all regions in the scene
        Region[] RegionArray = FindObjectsOfType<Region>();
        Regions = RegionArray.ToList();

        // Automatically update their GameObject names
        foreach (var region in Regions)
        {
            if (region != null)
            {
                region.gameObject.name = $"Region ({region.regionName})";
                EditorUtility.SetDirty(region);
            }
        }

        Repaint();
    }
}
