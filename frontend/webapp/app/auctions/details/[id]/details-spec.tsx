"use client";

import { Auction } from "@/app/models/auction";
import { Table } from "flowbite-react";
import React from "react";

interface Props {
  auction: Auction;
}

const DetailsSpec = ({ auction }: Props) => {
  return (
    <Table>
      <Table.Body>
        <Table.Row className="bg-white dark:border-gray-700 dark:bg-gray-800">
          <Table.Cell className="whitespace-nowrap font-semibold text-gray-900 dark:text-white">
            Seller
          </Table.Cell>
          <Table.Cell className="capitalize">{auction.seller}</Table.Cell>
        </Table.Row>
        <Table.Row className="bg-white dark:border-gray-700 dark:bg-gray-800">
          <Table.Cell className="whitespace-nowrap font-semibold text-gray-900 dark:text-white">
            Make
          </Table.Cell>
          <Table.Cell>{auction.item.make}</Table.Cell>
        </Table.Row>
        <Table.Row className="bg-white dark:border-gray-700 dark:bg-gray-800">
          <Table.Cell className="whitespace-nowrap font-semibold text-gray-900 dark:text-white">
            Model
          </Table.Cell>
          <Table.Cell>{auction.item.model}</Table.Cell>
        </Table.Row>
        <Table.Row className="bg-white dark:border-gray-700 dark:bg-gray-800">
          <Table.Cell className="whitespace-nowrap font-semibold text-gray-900 dark:text-white">
            Year manufactured
          </Table.Cell>
          <Table.Cell>{auction.item.year}</Table.Cell>
        </Table.Row>
        <Table.Row className="bg-white dark:border-gray-700 dark:bg-gray-800">
          <Table.Cell className="whitespace-nowrap font-semibold text-gray-900 dark:text-white">
            Mileage
          </Table.Cell>
          <Table.Cell>{auction.item.mileage}</Table.Cell>
        </Table.Row>
        <Table.Row className="bg-white dark:border-gray-700 dark:bg-gray-800">
          <Table.Cell className="whitespace-nowrap font-semibold text-gray-900 dark:text-white">
            Has reserve price?
          </Table.Cell>
          <Table.Cell>{auction.reservePrice > 0 ? "Yes" : "No"}</Table.Cell>
        </Table.Row>
      </Table.Body>
    </Table>
  );
};

export default DetailsSpec;
