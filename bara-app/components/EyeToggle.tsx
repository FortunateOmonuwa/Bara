"use client";

import Image from "next/image";

interface EyeToggleProps {
  isVisible: boolean;
  onToggle: () => void;
}

export default function EyeToggle({ isVisible, onToggle }: EyeToggleProps) {
  return (
    <button
      type="button"
      onClick={onToggle}
      className="absolute right-3 top-1/2 -translate-y-1/2"
    >
    
      <Image
        src="/Eye open.png" 
        alt={isVisible ? "Hide password" : "Show password"}
        width={20}
        height={20}
        className={`${!isVisible ? "opacity-50" : ""}`} 
      />
    </button>
  );
}
