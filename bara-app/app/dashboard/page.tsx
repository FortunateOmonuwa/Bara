"use client";

import Image from "next/image";
import Link from "next/link";
import DashboardNavbar from "@/components/DashboardNavbar";
import GenreDropdown from "@/components/GenreDropdown";

export default function DashboardPage() {
  return (
    <main className="min-h-screen bg-white">
      <DashboardNavbar />

      <div className="max-w-7xl mx-auto px-4 py-4">
        {/* Greeting */}
        <div className="flex items-center gap-2 mt-4">
          <h2 className="text-lg font-bold text-[#22242A]">Hello Jane!</h2>
          <Image src="/wave.png" alt="Wave" width={20} height={20} />
        </div>

        {/* Top row with dropdown */}
        <div className="flex items-center justify-between relative">
          <p className="text-sm text-[#22242A]">
            Explore powerful scripts, connect with talented writers.
          </p>

          {/* Genre Dropdown Component */}
          <GenreDropdown
            onChange={(selected) => {
              console.log("Selected genres: ", selected);
            }}
          />
        </div>
      </div>

      {/* Grid section */}
      <section className="max-w-7xl mx-auto px-4 py-6 pb-25">
        <div className="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 lg:grid-cols-4 gap-6 items-start">
          {Array.from({ length: 8 }).map((_, i) => (
            <div
              key={i}
              className={`
                group
                relative
                border border-[#ABADB2]
                rounded-md
                bg-white
                shadow-sm
                transition-all duration-300
                overflow-hidden
                h-[360px]
                hover:h-[430px]
                hover:shadow-md hover:bg-[#f9f9f9]
              `}
            >
              {/* Image */}
              <div className="relative">
                <Image
                  src="/flowery.png"
                  alt="Script"
                  width={400}
                  height={250}
                  className="w-full h-48 object-cover"
                />
                <span className="absolute top-3 left-3 bg-[#FFEDEE] text-[#810306] text-xs px-2 py-1 rounded border border-[#810306]">
                  Adventure
                </span>
                <button className="absolute top-3 right-3">
                  <Image src="/save.png" alt="Save" width={20} height={20} />
                </button>
              </div>

              {/* Content */}
              <div className="p-4 flex flex-col gap-2">
                <h3 className="text-base font-bold text-[#22242A]">
                  Broken Promise
                </h3>
                <p className="text-sm text-[#333740] leading-snug">
                  A desperate journalist uncovers a hidden AI network
                  controlling world events and must race against time to expose
                  the truth before becoming its next target.
                </p>
                <p className="text-base font-semibold text-[#333740]">
                  ₦300,000.00
                </p>

                {/* See More Button */}
                <Link href="/dashboard/scripts">
                  <button
                    className={`
                      mt-2 w-full bg-[#800000] text-white py-2 rounded
                      opacity-0
                      group-hover:opacity-100
                      transition-opacity duration-300
                    `}
                  >
                    See more
                  </button>
                </Link>
              </div>
            </div>
          ))}
        </div>
      </section>
    </main>
  );
}
