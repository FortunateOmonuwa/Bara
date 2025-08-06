"use client";

import DashboardNavbar from "@/components/DashboardNavbar";
import Link from "next/link";

export default function ViewSynopsis() {
  return (
    <div className="min-h-screen bg-white  text-[#22242A]  overflow-x-hidden">
      {/* Top Navigation */}
      <DashboardNavbar />

      {/* Page Content */}
      <div className="px-4 md:px-10 py-10 w-full space-y-10">
        {/* Title and Buttons */}
        <div className="flex justify-between items-start md:items-center flex-col md:flex-row">
          <h1 className="text-3xl font-semibold tracking-wide w-full md:w-auto">
            Broken Promise
          </h1>

          <div className="flex gap-4 mt-4 md:mt-0">
            <Link
              href="/dashboard/scripts/ViewScript"
              className="bg-[#810306] hover:bg-[#1a0000] text-white py-2 px-10 rounded-md text-sm font-medium"
            >
              View script
            </Link>
            <button className="border border-[#810306] text-[#810306] py-2 px-6 rounded-md text-sm font-medium hover:bg-[#fff5f5]">
              Message writer
            </button>
          </div>
        </div>

        {/* Synopsis Box */}
        <div className="max-w-[600px] mx-auto  border border-[#ABADB2] rounded-md p-6 md:p-10 space-y-4 text-sm md:text-base leading-relaxed ">
          <h2 className="text-lg font-semibold">Synopsis</h2>

          <p>
            In a bustling Nigerian city caught between tradition and modernity,
            Amaka, a gifted but overlooked playwright, discovers a mysterious
            journal buried beneath the floorboards of her late grandmother’s
            home. Inside the worn pages lie vivid details of a secret resistance
            movement that fought against colonial rule using underground theatre
            performances to spread messages of freedom — and Amaka’s grandmother
            was at the heart of it.
          </p>

          <p>
            Haunted by her own creative struggles and a growing sense of
            disconnection from her roots, Amaka becomes obsessed with finishing
            the play her grandmother never completed. But as she stages scenes
            inspired by the journal in her community theatre, strange things
            begin to happen — actors black out mid-performance, forgotten
            memories are triggered, and an anonymous sponsor demands the show go
            on, no matter the cost.
          </p>

          <p>
            Driven to uncover the full truth, Amaka’s investigation unravels a
            generational cover-up that could change her family’s legacy and the
            history of Nigerian arts forever. With the help of Obiora, a cynical
            historian turned script editor, she must navigate betrayals,
            spiritual awakenings, and the very real danger of uncovering secrets
            powerful people want to keep buried.
          </p>

          <p>
            “The Final Act” is a genre-blending script that fuses drama,
            historical mystery, and a touch of the supernatural. It explores
            identity, legacy, and the courage it takes to finish a story that
            was never yours to begin with — and what it means when art is both
            weapon and witness.
          </p>
        </div>
      </div>
    </div>
  );
}
