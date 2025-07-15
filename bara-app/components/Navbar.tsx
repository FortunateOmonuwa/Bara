// src/components/Navbar.tsx
import Link from "next/link";
import Logo from "./Logo";

export default function Navbar() {
  return (
    <nav className="w-full flex justify-between items-center py-4 px-8 bg-white">
    
      <Logo />

      <div className="flex items-center space-x-4">
        <Link
          href="/auth/login"
          className="text-barRedMain font-medium bg-white"
        >
          Log in
        </Link>
        <Link
          href="/auth/register"
          className="bg-red-300 text-white font-medium px-4 py-2 rounded-sm hover:bg-barRedBright1 transition-colors"
        >
          Create account
        </Link>
      </div>
    </nav>
  );
}
