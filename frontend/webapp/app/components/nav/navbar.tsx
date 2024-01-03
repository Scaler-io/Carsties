import React from "react";
import SearchBar from "./searchbar";
import LoginButton from "./login-button";
import Brand from "./brand";
import { getCurrentUser } from "@/app/services/auth.service";
import UserAction from "./user-action";

const Navbar = async () => {
  const user = await getCurrentUser();
  return (
    <header
      className="
        sticky top-0 z-50 flex justify-evenly bg-white p-5 items-center text-gray-800 shadow-sm
      ">
      <Brand />

      <SearchBar />

      {user ? <UserAction /> : <LoginButton />}
    </header>
  );
};

export default Navbar;
