const mongoose = require('mongoose');

const Schema = mongoose.Schema;
const DeviceSchema = new Schema({
    computerName: { type: String, required: true },
    GPUName: { type: String, required: true },
    numberOfLogicalProcessors: { type: String, required: true },
    chipset: { type: String, required: true },
    RAMSize: { type: Number, required: true },
    IPAddress: { type: String, required: true },
    MACAddress: { type: String, required: true },
    MACAddress: { type: String, required: true, unique: true },
    CPUUsage: { type: Number, required: true },
    numThread: { type: Number, required: true },
    numProcess: { type: Number, required: true },
    diskUsage: { type: Number, required: true },
    RAMUsage: { type: Number, required: true },
    GPUUsage: { type: Number },
    activeTime: { type: Number, required: true },
    isOnline: { type: Boolean, required: true },
});

module.exports = mongoose.model('Device', DeviceSchema);