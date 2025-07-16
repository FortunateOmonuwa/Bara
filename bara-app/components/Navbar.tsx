// src/components/Navbar.tsx
import Link from "next/link";
import Logo from "./Logo";

export default function Navbar() {
  return (
    <nav className="w-full flex justify-between items-center py-4 px-8 bg-white">
      <Logo />

      <div className="flex items-center space-x-10">
        <Link
          href="/auth/login"
          className="text-barRedMain font-medium  hover:text-[#BF0000]"
        >
          Log in
        </Link>
        <Link
          href="/auth/register"
          className="bg-[#800000] text-white font-medium px-10 py-3 rounded-md hover:bg-[#BF0000] transition-colors text-center"
        >
          Create account
        </Link>
      </div>
    </nav>
  );
}
