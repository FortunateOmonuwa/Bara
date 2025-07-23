"use client";

import Image from "next/image";
import Link from "next/link";

export default function DashboardNavbar() {
  return (
    <nav
      className="
        sticky top-0 z-50
        w-full bg-white
        px-4 md:px-8 py-3 flex items-center justify-between
        shadow-md
      "
    >
      {/* Left: Logo */}
      <div className="flex items-center gap-2">
        <Link href="/">
          <Image
            src="/logo.png"
            alt="Logo"
            width={50}
            height={50}
            className="object-contain"
          />
        </Link>
      </div>

      {/* Middle: Search bar */}
      <div className="flex-1 max-w-md mx-4 hidden md:flex">
        <div className="relative w-full">
          <input
            type="text"
            placeholder="Search writers and genres"
            className="w-full border border-[#ABADB2] rounded-md py-2 pl-10 pr-4 text-sm focus:outline-none focus:ring-1 focus:ring-[#800000] placeholder-[#858990]"
          />
          <Image
            src="/search-icon.png"
            alt="Search"
            width={24}
            height={24}
            className="absolute left-3 top-1/2 -translate-y-1/2"
          />
        </div>
      </div>

      {/* Right: Links */}
      <div className="flex items-center gap-6 text-sm font-semibold text-[#22242A]">
        <Link
          href="/account"
          className="hover:text-[#800000] flex items-center gap-1"
        >
          <Image src="/User_alt.png" alt="Account" width={16} height={16} />
          Account
        </Link>
        <Link
          href="/saved-scripts"
          className="hover:text-[#800000] flex items-center gap-1"
        >
          <Image src="/Shape.png" alt="Saved Scripts" width={16} height={16} />
          Saved scripts
        </Link>
        <Link
          href="/projects"
          className="hover:text-[#800000] flex items-center gap-1"
        >
          <Image
            src="/project-icon.png"
            alt="Projects"
            width={16}
            height={16}
          />
          My projects
        </Link>
      </div>
    </nav>
  );
}
