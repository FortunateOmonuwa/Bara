// src/components/Logo.tsx
import Image from "next/image";

export default function Logo() {
  return (
    <div className="flex items-center">
      <Image
        src="/logo.png"
        alt="Bara App Logo"
        width={79}
        height={79}
        priority
      />
    </div>
  );
}
