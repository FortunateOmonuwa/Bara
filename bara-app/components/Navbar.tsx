"use client";
import { useState } from "react";
import Link from "next/link";
import Logo from "./Logo";
import CreateAccountDropdown from "./CreateAccountDropdown";

export default function Navbar() {
  const [showDropdown, setShowDropdown] = useState(false);

  return (
    <nav className="w-full flex justify-between items-center py-4 px-8 bg-white relative">
      <Logo />

      <div className="flex items-center space-x-10">
        {/* Log in link */}
        <Link
          href="/auth/login"
          className="text-barRedMain font-medium hover:text-[#1a0000]"
        >
          Log in
        </Link>

        {/* Wrap button and dropdown in a relative container */}
        <div className="relative">
          <button
            onClick={() => setShowDropdown((prev) => !prev)}
            className="bg-[#800000] text-white font-medium px-10 py-3 rounded-md hover:bg-[#1a0000] transition-colors text-center"
          >
            Create account
          </button>

          {showDropdown && (
            <div className="absolute top-full right-0 mt-2">
              <CreateAccountDropdown onClose={() => setShowDropdown(false)} />
            </div>
          )}
        </div>
      </div>
    </nav>
  );
}
